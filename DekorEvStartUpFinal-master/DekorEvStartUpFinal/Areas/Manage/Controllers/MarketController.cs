using DekorEvStartUpFinal.DAL;
using DekorEvStartUpFinal.Models;
using DekorEvStartUpFinal.ViewModels.Market;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DekorEvStartUpFinal.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "SuperAdmin")]
    public class MarketController : Controller
    {
        private readonly DekorEvStartupAppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<AppUser> _userManager;

        public MarketController(DekorEvStartupAppDbContext context, IWebHostEnvironment env, UserManager<AppUser> userManager)
        {
            _context = context;
            _env = env;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(bool? status, int page = 1)
        {
            ViewBag.Status = status;

            IEnumerable<AppUser> Markets = await _context.AppUsers
            .Where(a=>a.isMarket&&a.EmailConfirmed&&(status!=null?a.isConfirmed==status:true)&&a.isMarket)
             //.Where(c => status != null ? c.IsDeleted == status : true)
            .ToListAsync();

            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)Markets.Count() / 5);

            return View(Markets.Skip((page - 1) * 5).Take(5));

        }

        public async Task<IActionResult> Deactivate(string? id, bool? status, int page = 1)
        {
            if (id == null)
            {
                return BadRequest();
            }

            AppUser dbAppUser = await _context.AppUsers.FirstOrDefaultAsync(a=>a.Id==id);
            List<Product> dbProduct=await _context.Products.Where(p=>p.AppUserId==id).ToListAsync();  
            if (dbAppUser == null) return BadRequest();


            dbAppUser.isConfirmed = false;
            foreach (Product product in dbProduct)
            {
                product.DeletedByAdmin = true;
            }
            await _context.SaveChangesAsync();
            ViewBag.Status = status;


            IEnumerable<AppUser> Markets = await _context.AppUsers.Where(a => a.isMarket && a.EmailConfirmed)
            .ToListAsync();


            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)Markets.Count() / 5);


            return PartialView("_MarketIndexPartial", Markets.Skip((page - 1) * 5).Take(5));
        }

        public async Task<IActionResult> Activate(string? id, bool? status, int page = 1)
        {
            if (id == null)
            {
                return BadRequest();
            }

            AppUser dbAppUser = await _context.AppUsers.FirstOrDefaultAsync(a => a.Id == id);
            List<Product> dbProduct = await _context.Products.Where(p => p.AppUserId == id).ToListAsync();
            if (dbAppUser == null) return BadRequest();

            dbAppUser.isConfirmed = true;

            foreach (Product product in dbProduct)
            {
                product.DeletedByAdmin = false;
            }


            await _context.SaveChangesAsync();
            ViewBag.Status = status;

            IEnumerable<AppUser> Markets = await _context.AppUsers.Where(a => a.isMarket && a.EmailConfirmed)
            .ToListAsync();


            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)Markets.Count() / 5);

            return PartialView("_MarketIndexPartial", Markets.Skip((page - 1) * 5).Take(5));
        }

        //public async Task<IActionResult> Detail(string? id, bool? status, int page = 1)
        //{
        //    AppUser appUser = await _userManager.FindByIdAsync(id);
        //    if (appUser == null) return NotFound();
        //    List<Product> products = await _context.Products.Where(p => p.AppUserId == appUser.Id && !p.IsDeleted && !p.DeletedByAdmin)
        //           .Include(p => p.ProductColorMaterials).ThenInclude(p => p.Color)
        //           .Include(p => p.ProductColorMaterials).ThenInclude(p => p.Material).
        //           ToListAsync();


        //    MarketDetailVM marketDetailVM = new MarketDetailVM
        //    {
        //        User = appUser,
        //        Products = products,
        //        ViewCounts = await _context.ViewCounts.FirstOrDefaultAsync(v => v.AppUserId == id)
        //    };



        //    return View(marketDetailVM);

        //}
       
    }





}
