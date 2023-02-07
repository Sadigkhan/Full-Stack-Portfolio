using DekorEvStartUpFinal.DAL;
using DekorEvStartUpFinal.Models;
using DekorEvStartUpFinal.ViewModels.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DekorEvStartUpFinal.Controllers
{
    //[Authorize(Roles = "Member")]
    public class OrderController : Controller
    {
        private readonly DekorEvStartupAppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public OrderController(DekorEvStartupAppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Create()
        {
            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedUserName == User.Identity.Name.ToUpperInvariant() && !u.isAdmin && !u.isMarket);
            if (appUser == null) return RedirectToAction("Login", "Account");

            double total = 0;

            List<Basket> baskets = await _context.Baskets
                .Include(b => b.Product)
                .Where(b => b.AppUserId == appUser.Id)
                .ToListAsync();

            ViewBag.Total = total;
            ViewBag.Basket = baskets;

            OrderVM orderVM = new OrderVM
            {
                FullName = appUser.FullName,
                Email = appUser.Email,
                City = appUser.City,
                Address = appUser.Adress
            };
            return View(orderVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderVM orderVM)
        {
            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedUserName == User.Identity.Name.ToUpperInvariant() && !u.isAdmin && !u.isMarket);

            if (appUser == null)
            {
                return RedirectToAction("Login", "Account");   
                    
            }

            List<Basket> baskets = await _context.Baskets
                 .Include(b => b.Product)
                 .Where(b => b.AppUserId == appUser.Id)
                 .ToListAsync();

            double total = 0;   

            ViewBag.Total=total;
            ViewBag.Basket = baskets;

            if (!ModelState.IsValid)
            {
                return View(orderVM);
            }

            List<OrderItem> orderItems = new List<OrderItem>();

            foreach (Basket item in baskets)
            {
                OrderItem orderItem = new OrderItem
                {
                    Count = item.Count,
                    ProductId = item.ProductId,
                    CreatedAt = DateTime.UtcNow.AddHours(4)

                };
                orderItems.Add(orderItem);
            }

            Order order = new Order
            {
                Address = orderVM.Address,
                AppUserId = appUser.Id,
                City = orderVM.City,
                OrderItems = orderItems,
                TotalPrice = total,
                CreatedAt = DateTime.UtcNow.AddHours(4)

            };

            List<ProductColorMaterial> colorMaterials = await _context.ProductColorMaterials.ToListAsync();


            foreach (Basket item in baskets)
            {
                foreach (ProductColorMaterial product in colorMaterials)
                {
                    if (product.MaterialId==item.MaterialId&&product.ColorId==item.ColorId&&product.ProductId==item.ProductId)
                    {
                        product.Count += item.Count;
                    }
                }
            }


            _context.Baskets.RemoveRange(baskets);
            HttpContext.Response.Cookies.Append("basket","");

            await _context.Orders.AddAsync(order);

            await _context.SaveChangesAsync();
            return RedirectToAction("index","home");
        }
    }
}
