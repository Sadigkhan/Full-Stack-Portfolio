using JuanBackFinal.DAL;
using JuanBackFinal.Extensions;
using JuanBackFinal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JuanBackFinal.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class BlogCategoryController : Controller
    {
        public readonly JuanAppDbContext _context;

        public BlogCategoryController(JuanAppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(bool? status, int page = 1)
        {
            ViewBag.Status = status;
            IEnumerable<BlogCategory> blogCategories = await _context.BlogCategories
                .Include(t => t.BlogToCategories)
                .Where(t => status != null ? t.IsDeleted == status : true)
                .OrderByDescending(t => t.CreatedAt).ToArrayAsync();

            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)blogCategories.Count() / 5);

            return View(blogCategories.Skip((page - 1) * 5).Take(5));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCategory blogCategory)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (string.IsNullOrWhiteSpace(blogCategory.Name))
            {
                ModelState.AddModelError("Name", "Blog Category can't be empty");
                return View();
            }


            if (blogCategory.Name.CheckString())
            {
                ModelState.AddModelError("Name", "Only Letters Allowed");
                return View(blogCategory);
            }


            if (await _context.BlogCategories.AnyAsync(t => t.Name.ToLower() == blogCategory.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "This BlogCategory name is already exists");
                return View();
            }

            blogCategory.CreatedAt = DateTime.UtcNow.AddHours(4);

            await _context.BlogCategories.AddAsync(blogCategory);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int? id, bool? status, int page = 1)
        {
            if (id == null) return BadRequest();

            BlogCategory blogCategory = await _context.BlogCategories.FirstOrDefaultAsync(t => t.Id == id);

            if (blogCategory == null) return NotFound();

            return View(blogCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, BlogCategory blogCategory, bool? status, int page = 1)
        {
            if (!ModelState.IsValid) return View(blogCategory);

            if (id == null) return BadRequest();

            if (id != blogCategory.Id) return BadRequest();

            BlogCategory dbBlogCategory = await _context.BlogCategories.FirstOrDefaultAsync(t => t.Id == id);

            if (dbBlogCategory == null) return NotFound();

            if (string.IsNullOrWhiteSpace(blogCategory.Name))
            {
                ModelState.AddModelError("Name", "Blog category Name Is Required");
                return View(dbBlogCategory);
            }

            if (blogCategory.Name.CheckString())
            {
                ModelState.AddModelError("Name", "Only Letters Allowed");
                return View(dbBlogCategory);
            }

            if (await _context.BlogCategories.AnyAsync(t => t.Id != blogCategory.Id && t.Name.ToLower() == blogCategory.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "This Blog category name is already exists");
                return View(dbBlogCategory);
            }

            dbBlogCategory.Name = blogCategory.Name;
            dbBlogCategory.UpdatedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { status = status, page = page });
        }

        public async Task<IActionResult> Delete(int? id, bool? status, int page = 1)
        {
            if (id == null) return BadRequest();

            BlogCategory dbBlogCategory = await _context.BlogCategories.FirstOrDefaultAsync(t => t.Id == id);

            if (dbBlogCategory == null) return NotFound();

            dbBlogCategory.IsDeleted = true;
            dbBlogCategory.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            ViewBag.Status = status;

            IEnumerable<BlogCategory> blogCategories = await _context.BlogCategories
                .Include(t => t.BlogToCategories)
                .Where(t => status != null ? t.IsDeleted == status : true)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)blogCategories.Count() / 5);



            return PartialView("_BlogCategoryIndexPartial", blogCategories.Skip((page - 1) * 5).Take(5));
        }

        public async Task<IActionResult> Restore(int? id, bool? status, int page = 1)
        {
            if (id == null) return BadRequest();

            BlogCategory dbBlogcategory = await _context.BlogCategories.FirstOrDefaultAsync(t => t.Id == id);

            if (dbBlogcategory == null) return NotFound();

            dbBlogcategory.IsDeleted = false;
            dbBlogcategory.DeletedAt = null;

            await _context.SaveChangesAsync();

            ViewBag.Status = status;

            IEnumerable<BlogCategory> blogCategories = await _context.BlogCategories
                .Include(t => t.BlogToCategories)
                .Where(t => status != null ? t.IsDeleted == status : true)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)blogCategories.Count() / 5);



            return PartialView("_BlogCategoryIndexPartial", blogCategories.Skip((page - 1) * 5).Take(5));
        }
    }
}
