using Kontakt.DAL;
using Kontakt.Models;
using Kontakt.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontakt.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public OrderController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Create()
        {
            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name && !u.isAdmin);

            if (appUser == null)
            {
                return Json(new { status = 400, message = $"İcazə verilməyən məlumatlar daxil etmisiniz" });
            }

            string cookieBasket = HttpContext.Request.Cookies["basket"];

            List<BasketVM> basketVMs = null;

            if (cookieBasket != null)
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(cookieBasket);
            }
            else
            {
                basketVMs = new List<BasketVM>();
            }

            foreach (BasketVM basketVM in basketVMs)
            {
                Product dbProduct = await _context.Products
                    .Include(x => x.Reviews)
                    .FirstOrDefaultAsync(p => p.Id == basketVM.ProductId);
                basketVM.Image = dbProduct.MainImage;
                basketVM.Price = (double)(dbProduct.DiscountPrice > 0 ? dbProduct.DiscountPrice : dbProduct.Price);
                basketVM.Title = dbProduct.Title;
                basketVM.Reviews = await _context.Reviews.Where(r => r.ProductId == dbProduct.Id).ToListAsync();
                basketVM.Product = dbProduct;
            }

            OrderVM orderVM = new OrderVM
            {
                Name = appUser.Name,
                Surname = appUser.SurName,
                Email = appUser.Email,
                Address = appUser.Address,
                City = appUser.City,
                Country = appUser.Country,
                State = appUser.State,
                ZipCode = appUser.ZipCode,
                Phone = appUser.PhoneNumber,



                BasketVMs = basketVMs

                
            };



            return View(orderVM);
        }

        [HttpPost]
       
        public async Task<IActionResult> CreateOrder([FromBody]  OrderVM orderVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name && !u.isAdmin);

            if (appUser == null)
            {
                return RedirectToAction("index", "home");
            }

            List<Basket> baskets = await _context.Baskets.Include(b => b.Product).Where(b => b.AppUserId == appUser.Id).ToListAsync();
            if (baskets.Count == 0)
            {
                return Json(new { status = 400, message = "Səbətiniz boşdur əvvəlcə məhsul seçin " });
            }
            if (appUser.PhoneNumber==null)
            {
                return Json(new { status = 400, message = "Telefon nömrəsi əlavə olunmayıb zəhmət olmasa əvvəlcə hesab məlumatlarından telefon nömrəsini əlavə edin" });
            }


            if (string.IsNullOrWhiteSpace(orderVM.State))
            {
                if (appUser.State==null)
                {
                    return Json(new { status = 400, message = "Rayon qeyd olunmayıb" });
                }
                else
                {
                    orderVM.State = appUser.State;
                }
                
            }
            if (string.IsNullOrWhiteSpace(orderVM.City))
            {
                if (appUser.City == null)
                {
                    return Json(new { status = 400, message = "Şəhər qeyd olunmayıb" });
                }
                else
                {
                    orderVM.City = appUser.City;
                }
                
            }
            if (string.IsNullOrWhiteSpace(orderVM.Country) )
            {
                if (appUser.Country == null)
                {
                    return Json(new { status = 400, message = "Ölkə qeyd olunmayıb" });
                }
                else
                {
                    orderVM.Country = appUser.Country;
                }
                
            }
            if (string.IsNullOrWhiteSpace(orderVM.ZipCode) )
            {
                if (appUser.ZipCode == null)
                {
                    return Json(new { status = 400, message = "Poçt kodu qeyd olunmayıb" });
                }
                else
                {
                    orderVM.ZipCode = appUser.ZipCode;
                }
                
            }
            if (string.IsNullOrWhiteSpace(orderVM.Address) )
            {
                if (appUser.Address == null)
                {
                    return Json(new { status = 400, message = "Ünvan qeyd olunmayıb" });

                }
                else
                {
                    orderVM.Address = appUser.Address;
                }
            }

            if (string.IsNullOrWhiteSpace(orderVM.Note))
            {
                orderVM.Note = null;
            }



            
            List<OrderItem> orderItems = new List<OrderItem>();
            double total = 0;

            foreach (Basket item in baskets)
            {
                total = (double)(total + (item.Count * (item.Product.DiscountPrice > 0 ? item.Product.DiscountPrice : item.Product.Price)));

                OrderItem orderItem = new OrderItem
                {
                    Count = item.Count,
                    Price = ((double)(item.Product.DiscountPrice > 0 ? item.Product.DiscountPrice : item.Product.Price)),
                    ProductId = item.ProductId,
                    TotalPrice = ((double)(item.Count * (item.Product.DiscountPrice > 0 ? item.Product.DiscountPrice : item.Product.Price))),
                    CreatedAt = DateTime.UtcNow.AddHours(4)
                };
                orderItems.Add(orderItem);
            }

            Order order = new Order
            {
                Address = orderVM.Address,
                AppUserId = appUser.Id,
                City = orderVM.City,
                Country = orderVM.Country,
                State = orderVM.State,
                TotalPrice = total,
                CreatedAt = DateTime.UtcNow.AddHours(4),
                ZipCode = orderVM.ZipCode,
                OrderItems = orderItems
            };

            _context.Baskets.RemoveRange(baskets);
            HttpContext.Response.Cookies.Delete("basket");
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return Json(new { status = 200, message = "" });
        }

        

        public async Task<IActionResult> GetMyOrder(int? orderId)
        {
            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name && !u.isAdmin);

            if (appUser == null) return BadRequest();

            if (orderId == null) return BadRequest();
            Order order = await _context.Orders

                .Include(o => o.OrderItems).ThenInclude(p => p.Product).ThenInclude(r=>r.Reviews)
                .Include(x=>x.OrderItems).ThenInclude(x=>x.Product).ThenInclude(x=>x.ProductDetails).ThenInclude(x=>x.DetailKey).ThenInclude(x=>x.DetailValues)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.AppUserId==appUser.Id);
            if (order == null) return NotFound();


            return PartialView("_MyOrderItemsPartial",order);
        }
    }
}
