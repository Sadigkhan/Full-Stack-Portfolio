using DekorEvStartUpFinal.DAL;
using DekorEvStartUpFinal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DekorEvStartUpFinal.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "SuperAdmin")]
    public class RemovablePremiumController : Controller
    {
        private readonly DekorEvStartupAppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public RemovablePremiumController(DekorEvStartupAppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index(int page = 1)
        {

            IEnumerable<Product> products = await _context.Products
             .Where(p => p.AppUser.NormalizedUserName != User.Identity.Name.ToUpperInvariant() && p.IsPremium && !p.IsDeleted)
             .Include(p => p.Category)
             .Include(p => p.ProductColorMaterials)
             .OrderByDescending(p => p.VipPaymentDate)
             .ToListAsync();

            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)products.Count() / 5);

            return View(products.Skip((page - 1) * 5).Take(5));
        }


        public async Task<IActionResult> RemovePremium(int? id, int page = 1)
        {

            if (id == null)
            {
                return BadRequest();
            }
            Product dbProduct = await _context.Products.FirstOrDefaultAsync(c => c.Id == id);
            if (dbProduct == null) return NotFound();

            dbProduct.IsPremium = false;
            dbProduct.IsFronted = false;
            dbProduct.IsVip = false;
            await _context.SaveChangesAsync();
            //ViewBag.Status = status;

            IEnumerable<Product> products = await _context.Products
                 .Where(p => p.AppUser.NormalizedUserName != User.Identity.Name.ToUpperInvariant() && p.IsPremium && !p.IsDeleted)
                .Include(p => p.Category)
                //.Include(s => s.ProductColorMaterials)
                //.Where(s => status != null ? s.IsVip == status : true)
                .OrderByDescending(s => s.VipPaymentDate)
                .ToListAsync();
            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)products.Count() / 5);

            return PartialView("_PremiumIndexPartial", products.Skip((page - 1) * 5).Take(5));
        }
    }
}
