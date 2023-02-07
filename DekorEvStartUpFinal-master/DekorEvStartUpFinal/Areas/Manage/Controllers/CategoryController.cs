using DekorEvStartUpFinal.DAL;
using DekorEvStartUpFinal.Extensions;
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
    [Area("manage")]
    [Authorize(Roles = "SuperAdmin")]
    public class CategoryController : Controller
    {
        private readonly DekorEvStartupAppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public CategoryController(DekorEvStartupAppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index(bool? status, bool? isMainRoute, int page = 1)
        {
            ViewBag.Status = status;
            ViewBag.IsMain = isMainRoute;

            IQueryable<Category> categories = _context.Categories
                .Include(t => t.Products)
                .OrderByDescending(t => t.CreatedAt);

            if (status != null)
                categories = categories.Where(c => c.IsDeleted == status);

            if (isMainRoute != null)
                categories = categories.Where(c => c.IsMain == isMainRoute);

            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)categories.Count() / 5);

            return View(await categories.Skip((page - 1) * 5).Take(5).ToListAsync());
        }
        public async Task<IActionResult> Create(bool? status, bool isMainRoute, int page = 1)
        {
            ViewBag.MainCategory = await _context.Categories.Where(c => c.IsMain && !c.IsDeleted).ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category, bool? status, bool isMainRoute, int page = 1)
        {
            ViewBag.MainCategory = await _context.Categories.Where(c => c.IsMain && !c.IsDeleted).ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (string.IsNullOrWhiteSpace(category.Name))
            {
                ModelState.AddModelError("Name", "Category name can't be empty");
                return View();
            }
            if (await _context.Categories.AnyAsync(c => c.Name.ToLower() == category.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "This category is already exists");
            }
            if (category.IsMain)
            {
                category.ParentId = null;
                if (category.CategoryImageFile == null)
                {
                    ModelState.AddModelError("CategoryImageFile", "Main category image is required");
                    return View();
                }
                if (!category.CategoryImageFile.CheckFileContentType("image/"))
                {
                    ModelState.AddModelError("CategoryImageFile", "Please choose image type file");
                    return View();
                }
                if (!category.CategoryImageFile.CheckFileSize(100))
                {
                    ModelState.AddModelError("CategoryImageFile", "File size must be less than 100 KB");
                    return View();
                }
                category.CategoryImage = category.CategoryImageFile.CreateFile(_env,"assets", "images");
            }
            else
            {
                if (category.ParentId == null)
                {
                    ModelState.AddModelError("ParentId", "Please choose main category");
                    return View();
                }
                if (!await _context.Categories.AnyAsync(c => c.Id == category.ParentId && !c.IsDeleted && c.IsMain))
                {
                    ModelState.AddModelError("ParentId", "This category doesn't exists");
                    return View();
                }
            }
            
            category.CreatedAt = DateTime.UtcNow.AddHours(4);
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { status = status, page = page });

        }
        public async Task<IActionResult> Detail(int? id, bool status, int page = 1)
        {
            if (id == null) return BadRequest();

            return View(await _context.Categories.Include(c => c.Children).FirstOrDefaultAsync(c => c.Id == id));

        }

        public async Task<IActionResult> Update(int? id, bool? status, bool? isMainRoute, int page = 1)
        {
            if (id == null) return BadRequest();
            Category category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null) return BadRequest();
            ViewBag.MainCategory = await _context.Categories.Where(c => c.Id != id && c.IsMain && !c.IsDeleted).ToListAsync();
            return View(category);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Category category, bool? status, bool? isMainRoute, int page = 1)
        {
            ViewBag.MainCategory = await _context.Categories.Where(c => c.Id != id && !c.IsDeleted).ToListAsync();

            Category dbCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (!ModelState.IsValid) return View(dbCategory);

            if (id != category.Id) return BadRequest();

            if (string.IsNullOrWhiteSpace(category.Name))
            {
                ModelState.AddModelError("Name", "Category name is required");
            }
            if (await _context.Categories.AnyAsync(c => c.Id != id && c.Name.ToLower() == category.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "This category is already exists");
                return View(dbCategory);
            }
            if (id == category.ParentId)
            {
                ModelState.AddModelError("Name", "Please choose right category");
                return View(dbCategory);
            }

           

            if (dbCategory == null) return NotFound();

            if (category.IsMain)
            {
                dbCategory.ParentId = null;

                if (category.CategoryImageFile != null)
                {
                    if (!category.CategoryImageFile.CheckFileContentType("image/"))
                    {
                        ModelState.AddModelError("CategoryImageFile", "Please choose image type files");
                        return View(category);
                    }
                    if (!category.CategoryImageFile.CheckFileSize(100))
                    {
                        ModelState.AddModelError("CategoryImageFile", "File size must be less than 100 KB");
                        return View(category);
                    }
                    if(dbCategory.CategoryImage != null)
                    {
                        Helper.Helper.DeleteFile(_env, dbCategory.CategoryImage, "assets", "images");
                    }
                    dbCategory.CategoryImage = category.CategoryImageFile.CreateFile(_env, "assets", "images");
                }

            }
            else
            {
                //if (category.CategoryImageFile == null)
                //{
                //    ModelState.AddModelError("CategoryImageFile", "Main category image is required");
                //    return View(category);
                //}
                if (category.ParentId == null)
                {
                    ModelState.AddModelError("ParentId", "Main category is required");
                    return View(dbCategory);
                }
                if (!await _context.Categories.AnyAsync(c => c.Id == category.ParentId && !c.IsDeleted && c.IsMain))
                {
                    ModelState.AddModelError("ParentId", "Chosen category doesn't exists");
                    return View(dbCategory);
                }
                dbCategory.ParentId = category.ParentId;
                Helper.Helper.DeleteFile(_env, "assets", "images");
                dbCategory.CategoryImage = null;
            }
            dbCategory.IsMain = category.IsMain;
            dbCategory.Name = category.Name;
            dbCategory.UpdatedAt = DateTime.UtcNow.AddHours(4);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { status = status, isMainRoute = isMainRoute, page = page });

        }
        public async Task<IActionResult> Delete(int? id, bool? status, bool? isMainRoute, int page = 1)
        {
            if (id == null) return BadRequest();

            Category dbcategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (dbcategory == null) return NotFound();

            dbcategory.IsDeleted = true;
            dbcategory.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();
            ViewBag.Status = status;
            ViewBag.IsMain = isMainRoute;

            IEnumerable<Category> categories = await _context.Categories
                .Include(c => c.Products)
                .Where(c => status != null ? c.IsDeleted == status : true && isMainRoute != null ? c.IsMain == isMainRoute : true)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            ViewBag.Pageindex = page;
            ViewBag.PageCount = Math.Ceiling((double)categories.Count() / 5);

            return PartialView("_CategoryIndexPartial", categories.Skip((page - 1) * 5).Take(5));


        }
        public async Task<IActionResult> Restore(int? id, bool? status, bool? isMainRoute, int page = 1)
        {
            if (id == null) return BadRequest();
            Category dbCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (dbCategory == null) return NotFound();
            dbCategory.IsDeleted = false;
            await _context.SaveChangesAsync();
            ViewBag.Status = status;
            ViewBag.IsMain = isMainRoute;

            IEnumerable<Category> categories = await _context.Categories
                .Include(c => c.Products)
                .Where(c => status != null ? c.IsDeleted == status : true && isMainRoute != null ? c.IsMain == isMainRoute : true)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)categories.Count() / 5);

            return PartialView("_CategoryIndexPartial", categories.Skip((page - 1) * 5).Take(5));

        }
    }
}
