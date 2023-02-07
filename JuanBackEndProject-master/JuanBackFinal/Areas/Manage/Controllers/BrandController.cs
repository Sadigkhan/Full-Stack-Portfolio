using DekorEvFinal.Helper;
using JuanBackFinal.DAL;
using JuanBackFinal.Extensions;
using JuanBackFinal.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JuanBackFinal.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class BrandController : Controller
    {

        private readonly JuanAppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public BrandController(JuanAppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult>  Index(bool? status,int page=1)
        {

            ViewBag.Status = status;
            IEnumerable<Brand> brands = await _context.Brands
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();


            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)brands.Count() / 5);
            return View(brands.Skip(page - 1).Take(5));
           
        }
        public async Task<IActionResult> Create(bool? status, int page = 1)
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Brand brand,bool? status, int page = 1)
        {
            if (brand.BrandImageFile != null)
            {
                if (!brand.BrandImageFile.CheckFileContentType("image/"))
                {
                    ModelState.AddModelError("BrandImageFile", "Only image type is allowed");
                    return View();
                }
                if (!brand.BrandImageFile.CheckFileSize(100))
                {
                    ModelState.AddModelError("BrandImageFile", "Image size can't be more than 100Kb");
                    return View();
                }
                brand.BrandImage = brand.BrandImageFile.CreateFile(_env, "assets", "img", "brand");
            }
            else
            {
                ModelState.AddModelError("BrandImageFile", "Brand Image is required");
                return View();
            }

            brand.CreatedAt = DateTime.UtcNow.AddHours(4);


            await _context.Brands.AddAsync(brand);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { status, page });
        }


        public async Task<IActionResult> Update(int? id,bool? status, int page = 1)
        {
            if (id == null) return BadRequest();



            Brand brand = await _context.Brands
                 .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (brand==null)
            {
                return NotFound();
            }

            return View(brand);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int ? id,Brand brand,bool? status,int page = 1)
        {
            if (id == null) return BadRequest();
            if (id != brand.Id)
            {
                return BadRequest();
            }

            Brand dbBrand = await _context.Brands
                  .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (dbBrand == null) return NotFound();

            if (!ModelState.IsValid) return View(dbBrand);


            if (brand.BrandImageFile != null)
            {
                if (!brand.BrandImageFile.CheckFileContentType("image/"))
                {
                    ModelState.AddModelError("MainImageFile", "Only image type is allowed");
                    return View();
                }
                if (!brand.BrandImageFile.CheckFileSize(100))
                {
                    ModelState.AddModelError("MainImageFile", "Image size can't be more than 100Kb");
                    return View();
                }
                Helper.DeleteFile(_env, dbBrand.BrandImage, "assets", "img", "product");
                dbBrand.BrandImage = brand.BrandImageFile.CreateFile(_env, "assets", "img", "brand");
            }

            dbBrand.UpdatedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return RedirectToAction("index", new { status, page });
        }

        public async Task<IActionResult> Delete(int? id, bool? status, int page = 1)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Brand dbBrand = await _context.Brands.FirstOrDefaultAsync(c => c.Id == id);
            if (dbBrand == null)
            {
                return NotFound();
            }
            dbBrand.IsDeleted = true;
            dbBrand.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();
            ViewBag.Status = status;


            IEnumerable<Brand> brands = await _context.Brands
              
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)brands.Count() / 5);
            return PartialView("_BrandIndexPartial", brands.Skip((page - 1) * 5).Take(5));
        }

        public async Task<IActionResult> Restore(int? id, bool? status, int page = 1)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Brand dbBrand = await _context.Brands.FirstOrDefaultAsync(c => c.Id == id);
            if (dbBrand == null)
            {
                return NotFound();
            }
            dbBrand.IsDeleted = false;
            

            await _context.SaveChangesAsync();
            ViewBag.Status = status;


            IEnumerable<Brand> brands = await _context.Brands

                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)brands.Count() / 5);
            return PartialView("_BrandIndexPartial", brands.Skip((page - 1) * 5).Take(5));
        }
    }
}
