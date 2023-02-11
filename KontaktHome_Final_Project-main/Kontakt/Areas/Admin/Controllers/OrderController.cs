using Kontakt.DAL;
using Kontakt.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Kontakt.Helpers.Helper;

namespace Kontakt.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public OrderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(bool? status, int page = 1)
        {
            ViewBag.Status = status;

            IQueryable<Order> orders = _context.Orders
                .Include(x=>x.OrderItems).ThenInclude(x=>x.Product)
                .Include(x=>x.AppUser)
                .OrderByDescending(c => c.CreatedAt)
                .AsQueryable();

            if (status != null)
                orders = orders.Where(c => c.IsDeleted == status);

            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)orders.Count() / 5);
            return View(orders.Skip((page - 1) * 5).Take(5).ToList());
        }

        public IActionResult PageChange(string key,string UserName, bool? status, int page = 1)
        {




            IQueryable<Order> orders = _context.Orders
                  .Include(x => x.OrderItems).ThenInclude(x => x.Product)
                  .Include(x => x.AppUser)
                  .OrderByDescending(c => c.CreatedAt)
                  .AsQueryable();

            if (status != null)
            {
                orders = orders.Where(c => c.IsDeleted == status);
            }

            if (key != null)
            {
                orders = orders.Where(c =>c.Id.ToString().Contains(key));
            }
            if (UserName != null)
            {
                orders = orders.Where(c => c.AppUser.Name.ToLower().Contains(UserName));
            }

            ViewBag.Status = status;
            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)orders.Count() / 5);
            return PartialView("_OrderIndexPartial", orders.Skip((page - 1) * 5).Take(5));

        }

        public async Task<IActionResult> UpdateOrder(int?id)
        {
            
            if (id == null) return BadRequest();
            Order order = await _context.Orders
                .Include(o => o.OrderItems).ThenInclude(p => p.Product).ThenInclude(r => r.Reviews)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return NotFound();


            return View(order);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrder(int? id, int orderStatus, bool? status, int page = 1)
        {
            if (id == null) return BadRequest();

            Order order = await _context.Orders
                .Include(o => o.OrderItems).ThenInclude(p => p.Product).ThenInclude(r => r.Reviews)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();

            if (order.Status != OrderStatus.Təsdiqlənən && orderStatus == 1)
            {
                foreach (var item in order.OrderItems)
                {
                    if (item.Product.Count>item.Count)
                    {
                        item.Product.Count -= item.Count;
                    }
                    else
                    {
                        ModelState.AddModelError("", $"{item.Product.Title}-Məhsul seçilən say qədər yoxdur");
                     
                        return View(order);
                    }
                    
                    
                }
            }

            order.Status = (OrderStatus)orderStatus;
            order.UpdatedAt = DateTime.UtcNow.AddHours(4);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { status = status, page = page });
        }
    }
}
