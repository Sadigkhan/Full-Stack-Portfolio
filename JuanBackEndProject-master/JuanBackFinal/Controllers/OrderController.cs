using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JuanBackFinal.DAL;
using JuanBackFinal.Models;
using JuanBackFinal.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P224Allup.Controllers
{
    [Authorize(Roles ="Member,SuperAdmin")]
    public class OrderController : Controller
    {
        private readonly JuanAppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public OrderController(JuanAppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Create()
        {
            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u=>u.UserName == User.Identity.Name && !u.IsAdmin);

            if (appUser == null)
            {
                return RedirectToAction("login", "Account");
            }

            double total = 0;
            List<Basket> baskets = await _context.Baskets.Include(b=>b.Product).Where(b => b.AppUserId == appUser.Id).ToListAsync();

            foreach (Basket item in baskets)
            {
                total = total + (item.Count * (item.Product.DiscountPrice > 0 ? item.Product.DiscountPrice : item.Product.SalePrice));
            }

            ViewBag.Total = total;

            OrderVM orderVM = new OrderVM
            {
                FullName = appUser.FullName,
                Email = appUser.Email,
                Address = appUser.Address,
                City = appUser.City,
                Country = appUser.Country,
                State = appUser.State,
                ZipCode = appUser.ZipCode
            };

            return View(orderVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderVM orderVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name && !u.IsAdmin);

            if (appUser == null)
            {
                return RedirectToAction("login", "Account");
            }

            List<Basket> baskets = await _context.Baskets.Include(b => b.Product).Where(b => b.AppUserId == appUser.Id).ToListAsync();
            List<OrderItem> orderItems = new List<OrderItem>();
            double total = 0;

            foreach (Basket item in baskets)
            {
                total = total + (item.Count * (item.Product.DiscountPrice > 0 ? item.Product.DiscountPrice : item.Product.SalePrice));

                OrderItem orderItem = new OrderItem
                {
                    Count = item.Count,
                    Price = (item.Product.DiscountPrice > 0 ? item.Product.DiscountPrice : item.Product.SalePrice),
                    ProductId = item.ProductId,
                    TotalPrice = (item.Count * (item.Product.DiscountPrice > 0 ? item.Product.DiscountPrice : item.Product.SalePrice)),
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
            HttpContext.Response.Cookies.Append("basket", "");
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return RedirectToAction("index", "home");
        }
    }
}
