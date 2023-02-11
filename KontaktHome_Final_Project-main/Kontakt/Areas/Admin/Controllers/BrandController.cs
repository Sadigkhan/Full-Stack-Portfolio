using Kontakt.DAL;
using Kontakt.Extensions;
using Kontakt.Helpers;
using Kontakt.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontakt.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Publisher,Manager")]
    public class BrandController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public BrandController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(bool? status, int page = 1)
        {
            ViewBag.Status = status;

            IQueryable<Brand> brands = _context.Brands
                .OrderByDescending(c => c.CreatedAt)
                .AsQueryable();

            if (status != null)
                brands = brands.Where(c => c.IsDeleted == status);

            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)brands.Count() / 9);
            return View(brands.Skip((page - 1) * 8).Take(8).ToList());
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.SubCategory = await _context.Categories.Where(c => !c.IsMain && !c.IsDeleted).ToListAsync();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Brand brand, bool? status, bool? isMainRoute, int page = 1)
        {
            ViewBag.SubCategory = await _context.Categories.Where(c => !c.IsMain && !c.IsDeleted).ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (string.IsNullOrWhiteSpace(brand.Name))
            {
                ModelState.AddModelError("Name", "Bosluq Olmamalidir");
                return View();
            }

            if (await _context.Brands.AnyAsync(t => t.Name.ToLower() == brand.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "Alreade Exists");
                return View();
            }
            if (brand.ImageFile!=null)
            {
                if (!brand.ImageFile.CheckFileContentType("image/png"))
                {
                    ModelState.AddModelError("ImageFile", "Secilen Seklin Novu Uygun");
                    return View();
                }

                if (!brand.ImageFile.CheckFileSize(5000))
                {
                    ModelState.AddModelError("CategoryImage", "Secilen Seklin Olcusu Maksimum 500 Kb Ola Biler");
                    return View();
                }

                brand.ImageUrl = brand.ImageFile.CreateFile(_env, "user", "assets", "img", "BrandImg");
            }
            else
            {
                ModelState.AddModelError("ImageFile", "Sekil Vacibdir");
                return View();
            }

            brand.CreatedAt = DateTime.UtcNow.AddHours(4);
            await _context.Brands.AddAsync(brand);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { status = status, isMainRoute = isMainRoute, page = page });
        }
        public async Task<IActionResult> Detail(int? id, bool? status, int page = 1)
        {
            ViewBag.Status = status;
            ViewBag.PageIndex = page;
            if (id == null) return BadRequest();

            Brand brand = await _context.Brands
                .Include(b=>b.CategoryBrands).ThenInclude(c=>c.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (brand == null) return NotFound();

            return View(brand);
        }

        public async Task<IActionResult> Update(int? id, bool? status, int page = 1)
        {
            ViewBag.Status = status;
            ViewBag.PageIndex = page;
            if (id == null) return BadRequest();

            Brand brand = await _context.Brands.FirstOrDefaultAsync(c => c.Id == id);

            if (brand == null) return NotFound();

           ViewBag.SubCategory = await _context.Categories.Where(c => !c.IsMain && !c.IsDeleted).ToListAsync();


            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Brand brand, bool? status, bool? isMainRoute, int page = 1)
        {
            

            Brand dbBrand = await _context.Brands.FirstOrDefaultAsync(c => c.Id == id);

            if (dbBrand == null) return NotFound();

            if (!ModelState.IsValid)
            {
                return View(dbBrand);
            }

            if (id != brand.Id) return BadRequest();

            if (string.IsNullOrWhiteSpace(brand.Name))
            {
                ModelState.AddModelError("Name", "Bosluq Olmamalidir");
                return View(dbBrand);
            }


            if (await _context.Brands.AnyAsync(t => t.Id != id && t.Name.ToLower() == brand.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "Alreade Exists");
                return View(dbBrand);
            }

            if (brand.ImageFile != null)
            {
                if (!brand.ImageFile.CheckFileContentType("image/png"))
                {
                    ModelState.AddModelError("ImageFile", "Secilen Seklin Novu Uygun");
                    return View(dbBrand);
                }

                if (!brand.ImageFile.CheckFileSize(5000))
                {
                    ModelState.AddModelError("ImageFile", "Secilen Seklin Olcusu Maksimum 500 Kb Ola Biler");
                    return View(dbBrand);
                }

                if (dbBrand.ImageUrl != null)
                {
                    Helper.DeleteFile(_env, dbBrand.ImageUrl, "user", "assets", "img", "BrandImg");
                }

                dbBrand.ImageUrl = brand.ImageFile.CreateFile(_env, "user", "assets", "img", "BrandImg");

            }


            dbBrand.Name = brand.Name;
            dbBrand.UpdatedAt = DateTime.UtcNow.AddHours(4);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { status = status, isMainRoute = isMainRoute, page = page });
        }

        public async Task<IActionResult> DeleteRestore(int? id, bool? status, int page = 1)
        {
            if (id == null) return BadRequest();

            Brand dbBrand = await _context.Brands.FirstOrDefaultAsync(t => t.Id == id);

            if (dbBrand == null) return NotFound();

            if (dbBrand.IsDeleted)
            {
                dbBrand.IsDeleted = false;
            }
            else
            {
                dbBrand.IsDeleted = true;
                dbBrand.DeletedAt = DateTime.UtcNow.AddHours(4);
            }


            await _context.SaveChangesAsync();

            ViewBag.Status = status;
            IEnumerable<Brand> brands = await _context.Brands
                .Include(t => t.Products)
                .Where(t => status != null ? t.IsDeleted == status : true)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)brands.Count() / 8);
            return PartialView("_BrandIndexPartial", brands.Skip((page - 1) * 8).Take(8));

        }

        public IActionResult PageChange(string key, bool? status, int page = 1)
        {




            IQueryable<Brand> brands = _context.Brands
                .Include(c => c.Products)
                .OrderByDescending(c => c.CreatedAt)
                .AsQueryable();

            if (status != null)
            {
                brands = brands.Where(c => c.IsDeleted == status);
            }

            if (key != null)
            {
                brands = brands.Where(c => c.Name.ToLower().Contains(key.ToLower()));
            }


            ViewBag.Status = status;
            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)brands.Count() / 8);
            return PartialView("_BrandIndexPartial", brands.Skip((page - 1) * 8).Take(8));

        }

        #region Brand Vs Category

        public IActionResult BrandVsCategory(bool? status, int page = 1)
        {
            ViewBag.Status = status;

            IQueryable<CategoryBrand> categoryBrands = _context.CategoryBrands
                .Include(c=>c.Category)
                .Include(b=>b.Brand).ThenInclude(p=>p.Products)
                .OrderByDescending(c => c.CreatedAt)
                .AsQueryable();

            if (status != null)
                categoryBrands = categoryBrands.Where(c => c.IsDeleted == status);

            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)categoryBrands.Count() / 5);
            return View(categoryBrands.Skip((page - 1) * 5).Take(5).ToList());
        }
        public async Task<IActionResult> BrandVsCategoryCreate()
        {
            ViewBag.MainCategory = await _context.Categories.Where(c => c.IsMain && !c.IsDeleted).ToListAsync();
            ViewBag.Brand = await _context.Brands.Where(c =>!c.IsDeleted).ToListAsync();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BrandVsCategoryCreate(CategoryBrand categoryBrand)
        {
            ViewBag.MainCategory = await _context.Categories.Where(c => c.IsMain && !c.IsDeleted).ToListAsync();
            ViewBag.Brand = await _context.Brands.Where(c => !c.IsDeleted).ToListAsync();
            CategoryBrand DbcategoryBrand = await _context.CategoryBrands.Where(c => c.CategoryId == categoryBrand.CategoryId && c.BrandId==categoryBrand.BrandId).FirstOrDefaultAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (categoryBrand.BrandId == null)
            {
                ModelState.AddModelError("Brand", "Brand bos ola bilmez");
                return View();
            }
            if (categoryBrand.CategoryId==null)
            {
                ModelState.AddModelError("Category", "Categorya bos ola bilmez");
                return View();
            }
            if (DbcategoryBrand!=null)
            {
                ModelState.AddModelError("Category", "Bu Categorya artiq secilib");
                return View();
            }
            if (categoryBrand.ImageFile!=null)
            {
                if (!categoryBrand.ImageFile.CheckFileContentType("image/png"))
                {
                    ModelState.AddModelError("ImageFile", "Secilen Seklin Novu Uygun");
                    return View();
                }

                if (!categoryBrand.ImageFile.CheckFileSize(5000))
                {
                    ModelState.AddModelError("CategoryImage", "Secilen Seklin Olcusu Maksimum 500 Kb Ola Biler");
                    return View();
                }

                categoryBrand.ImageUrl = categoryBrand.ImageFile.CreateFile(_env, "user", "assets", "img", "BrandImg");
            }
            else
            {
                ModelState.AddModelError("ImageFile", "Sekil Vacibdir");
                return View();
            }
            categoryBrand.CreatedAt = DateTime.UtcNow.AddHours(4);
            await _context.CategoryBrands.AddAsync(categoryBrand);
            await _context.SaveChangesAsync();

            return RedirectToAction("BrandVsCategory");
        }
        public async Task<IActionResult> BrandVsCategoryUpdate(int? id, bool? status, int page = 1)
        {
            ViewBag.Status = status;
            ViewBag.PageIndex = page;
            if (id == null) return BadRequest();

            CategoryBrand categoryBrand = await _context.CategoryBrands.FirstOrDefaultAsync(c => c.Id == id);

            if (categoryBrand == null) return NotFound();

            ViewBag.SubCategory = await _context.Categories.Where(c => !c.IsMain && !c.IsDeleted).ToListAsync();
            ViewBag.Brand = await _context.Brands.Where(c => !c.IsDeleted).ToListAsync();


            return View(categoryBrand);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BrandVsCategoryUpdate(int? id, CategoryBrand categoryBrand, bool? status, bool? isMainRoute, int page = 1)
        {


            CategoryBrand DbcategoryBrand = await _context.CategoryBrands.FirstOrDefaultAsync(c => c.Id == id);

            if (DbcategoryBrand == null) return NotFound();

            if (!ModelState.IsValid)
            {
                return View(DbcategoryBrand);
            }

            if (id != categoryBrand.Id) return BadRequest();

           

            if (categoryBrand.ImageFile != null)
            {
                if (!categoryBrand.ImageFile.CheckFileContentType("image/png"))
                {
                    ModelState.AddModelError("ImageFile", "Secilen Seklin Novu Uygun");
                    return View(DbcategoryBrand);
                }

                if (!categoryBrand.ImageFile.CheckFileSize(5000))
                {
                    ModelState.AddModelError("ImageFile", "Secilen Seklin Olcusu Maksimum 500 Kb Ola Biler");
                    return View(DbcategoryBrand);
                }

                if (DbcategoryBrand.ImageUrl != null)
                {
                    Helper.DeleteFile(_env, DbcategoryBrand.ImageUrl, "user", "assets", "img", "BrandImg");
                }

                DbcategoryBrand.ImageUrl = categoryBrand.ImageFile.CreateFile(_env, "user", "assets", "img", "BrandImg");

            }


            
            DbcategoryBrand.UpdatedAt = DateTime.UtcNow.AddHours(4);
            await _context.SaveChangesAsync();

            return RedirectToAction("BrandVsCategory", new { status = status, isMainRoute = isMainRoute, page = page });
        }

        public async Task<IActionResult> DeleteRestoreBrand(int? id, bool? status, int page = 1)
        {
            if (id == null) return BadRequest();

            CategoryBrand dbCategoryBrand = await _context.CategoryBrands.FirstOrDefaultAsync(t => t.Id == id);

            if (dbCategoryBrand == null) return NotFound();

            if (dbCategoryBrand.IsDeleted)
            {
                dbCategoryBrand.IsDeleted = false;
                dbCategoryBrand.DeletedAt = null;
            }
            else
            {
                dbCategoryBrand.IsDeleted = true;
                dbCategoryBrand.DeletedAt = DateTime.UtcNow.AddHours(4);
            }


            await _context.SaveChangesAsync();

            ViewBag.Status = status;
            IEnumerable<CategoryBrand> categoryBrands = await _context.CategoryBrands
                .Include(t => t.Category)
                .Include(t=>t.Brand).ThenInclude(p => p.Products)
                .Where(t => status != null ? t.IsDeleted == status : true)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)categoryBrands.Count() / 5);
            return PartialView("_BrandVsCategoryIndexPartial", categoryBrands.Skip((page - 1) * 5).Take(5));

        }
        public IActionResult PageChangeBrand(string key, string keyCtg, bool? status, int page = 1)
        {




            IQueryable<CategoryBrand> categoryBrands = _context.CategoryBrands
                 .Include(c => c.Category)
                 .Include(b => b.Brand).ThenInclude(p => p.Products)
                 .OrderByDescending(c => c.CreatedAt)
                 .AsQueryable();

            if (status != null)
            {
                categoryBrands = categoryBrands.Where(c => c.IsDeleted == status);
            }

            if (key != null)
            {
                categoryBrands = categoryBrands.Where(c => c.Brand.Name.ToLower().Contains(key.ToLower()));
            }
            if (keyCtg != null)
            {
                categoryBrands = categoryBrands.Where(c => c.Category.Name.ToLower().Contains(keyCtg.ToLower()));
            }


            ViewBag.Status = status;
            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)categoryBrands.Count() / 5);
            return PartialView("_BrandVsCategoryIndexPartial", categoryBrands.Skip((page - 1) * 5).Take(5));

        }
        #endregion
    }
}
