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
    public class BasketController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public BasketController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
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
                Product dbProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == basketVM.ProductId);
                basketVM.Image = dbProduct.MainImage;
                basketVM.Price = (double)(dbProduct.DiscountPrice > 0 ? dbProduct.DiscountPrice : dbProduct.Price);
                basketVM.Title = dbProduct.Title;
            }

            return View(basketVMs);
        }

        public async Task<IActionResult> GetMiniBasket()
        {
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
                    .Include(x=>x.ProductDetails).ThenInclude(x=>x.DetailKey).ThenInclude(x=>x.DetailValues).OrderBy(x=>x.UpdatedAt)
                    .Include(x=>x.Reviews)
                    .FirstOrDefaultAsync(p => p.Id == basketVM.ProductId);
                basketVM.Image = dbProduct.MainImage;
                basketVM.Price = (double)(dbProduct.DiscountPrice > 0 ? dbProduct.DiscountPrice : dbProduct.Price);
                basketVM.Title = dbProduct.Title;
                basketVM.Reviews = await _context.Reviews.Where(r => r.ProductId == dbProduct.Id).ToListAsync();
                basketVM.Product = dbProduct;
            }

            

            return PartialView("_GetMiniBasket", basketVMs);
        }

        public async Task<IActionResult> GetOrderBasket()
        {
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



            return PartialView("_GetOrderBasketPartial", basketVMs);
        }
        public async Task<IActionResult> GetBasketCount()
        {
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

            return Json(new { status = 200, message = $"{basketVMs.Count}" });
        }

        public async Task<IActionResult> DeleteBasket(int? id)
        {
            if (id == null) return BadRequest();

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            string cookieBasket = HttpContext.Request.Cookies["basket"];

            List<BasketVM> basketVMs = null;

            if (cookieBasket != null)
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(cookieBasket);

                BasketVM basketVM = basketVMs.FirstOrDefault(b => b.ProductId == id);

                if (basketVM == null)
                {
                    return NotFound();
                }

                basketVMs.Remove(basketVM);
            }
            else
            {
                return BadRequest();
            }

            cookieBasket = JsonConvert.SerializeObject(basketVMs);
            HttpContext.Response.Cookies.Append("basket", cookieBasket);

            foreach (BasketVM basketVM in basketVMs)
            {
                Product dbProduct = await _context.Products
                    .Include(x=>x.ProductDetails).ThenInclude(x=>x.DetailKey).ThenInclude(x=>x.DetailValues)
                     .Include(x => x.Reviews)
                     .FirstOrDefaultAsync(p => p.Id == basketVM.ProductId);
                basketVM.Image = dbProduct.MainImage;
                basketVM.Price = (double)(dbProduct.DiscountPrice > 0 ? dbProduct.DiscountPrice : dbProduct.Price);
                basketVM.Title = dbProduct.Title;
                basketVM.Reviews = await _context.Reviews.Where(r => r.ProductId == dbProduct.Id).ToListAsync();
                basketVM.Product = dbProduct;
            }
            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name && !u.isAdmin);

            if (appUser!=null)
            {
                List<Basket> existedBasket = await _context.Baskets.Where(b => b.AppUserId == appUser.Id).ToListAsync();

                if (existedBasket.Any(b => b.ProductId == id))
                {
                    foreach (Basket basket in existedBasket)
                    {
                        if (basket.ProductId == id)
                        {
                            _context.Baskets.Remove(basket);
                        }
                        
                    }
                    await _context.SaveChangesAsync();
                }
            }


            return PartialView("_GetMiniBasket", basketVMs);
        }
        public async Task<IActionResult> OneEditBasket(int? id,bool key)
        {
            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name && !u.isAdmin);
            if (id == null) return BadRequest();

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            string cookieBasket = HttpContext.Request.Cookies["basket"];

            List<BasketVM> basketVMs = null;

            if (cookieBasket != null)
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(cookieBasket);

                BasketVM basketVM = basketVMs.FirstOrDefault(b => b.ProductId == id);

                if (basketVM == null)
                {
                    return NotFound();
                }

                if (key==true)
                {
                    if (basketVM.Count > 1)
                    {
                        basketVM.Count -= 1;

                        if (appUser != null)
                        {
                            List<Basket> existedBasket = await _context.Baskets.Where(b => b.AppUserId == appUser.Id).ToListAsync();

                            if (existedBasket.Any(b => b.ProductId == id))
                            {
                                foreach (Basket basket in existedBasket)
                                {
                                    if (basket.ProductId == id)
                                    {
                                        basket.Count -= 1;
                                        basket.UpdatedAt = DateTime.UtcNow.AddHours(4);
                                    }
                                    
                                }
                                
                               
                            }
                        }
                    }
                }
                else if (key==false)
                {
                    if (basketVM.Count < product.Count)
                    {
                        basketVM.Count += 1;

                        

                        if (appUser != null)
                        {
                            List<Basket> existedBasket = await _context.Baskets.Where(b => b.AppUserId == appUser.Id).ToListAsync();

                            if (existedBasket.Any(b => b.ProductId == id))
                            {
                                foreach (Basket basket in existedBasket)
                                {
                                    if (basket.ProductId == id)
                                    {
                                        basket.Count += 1;
                                        basket.UpdatedAt = DateTime.UtcNow.AddHours(4);
                                    }

                                }

                              
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }
            else
            {
                return BadRequest();
            }

            cookieBasket = JsonConvert.SerializeObject(basketVMs);
            HttpContext.Response.Cookies.Append("basket", cookieBasket);

            foreach (BasketVM basketVM in basketVMs)
            {
                Product dbProduct = await _context.Products
                     .Include(x => x.Reviews)
                       .Include(x => x.ProductDetails).ThenInclude(x => x.DetailKey).ThenInclude(x => x.DetailValues)
                     .FirstOrDefaultAsync(p => p.Id == basketVM.ProductId);
                basketVM.Image = dbProduct.MainImage;
                basketVM.Price = (double)(dbProduct.DiscountPrice > 0 ? dbProduct.DiscountPrice : dbProduct.Price);
                basketVM.Title = dbProduct.Title;
                basketVM.Reviews = await _context.Reviews.Where(r => r.ProductId == dbProduct.Id).ToListAsync();
                basketVM.Product = dbProduct;
            }

            return PartialView("_GetMiniBasket", basketVMs);
        }
        
    }
}
