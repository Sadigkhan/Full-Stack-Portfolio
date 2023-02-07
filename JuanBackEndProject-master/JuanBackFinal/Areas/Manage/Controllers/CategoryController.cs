using JuanBackFinal.DAL;
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
    public class CategoryController : Controller
    {
        private readonly JuanAppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CategoryController(JuanAppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index(bool? status,int page=1)
        {
            ViewBag.Status = status;
            IEnumerable<Category> categories = await _context.Categories
                .Where(c => status != null ? c.IsDeleted == status : true)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)categories.Count() / 5);
            return View(categories.Skip((page-1)*5).Take(5));
        }
        public async Task<IActionResult> Create(bool? status, int page = 1)
        {
            ViewBag.Category = await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category,bool? status, int page = 1)
        {
            ViewBag.Category = await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();
            if(!ModelState.IsValid) return View();

            if (string.IsNullOrWhiteSpace(category.Name))
            {
                ModelState.AddModelError("Name", "Category name cant't be empty");
                return View();
            }
            if (await _context.Categories.AnyAsync(c=>c.Name.ToLower()==category.Name.ToLower()))
            {
                ModelState.AddModelError("Name","This name is already taken");
                return View();
            }
            category.CreatedAt = DateTime.UtcNow.AddHours(4);
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { status=status,page=page});
        }
        //public async Task<IActionResult> Detail(int? id,bool? status,int page = 1)
        //{
        //    if (id==null)
        //    {
        //        return BadRequest();
        //    }
        //    if(await _context.Categories.Include(c => c.Products).ThenInclude(p => p.CategoryId == id).ToListAsync() == null)
        //    {
        //        return View();
        //    }
        //    return View(await _context.Categories.Include(c => c.Products).ThenInclude(p => p.CategoryId == id).ToListAsync());
        //}
        public async Task<IActionResult> Update(int? id,bool? status,int page=1)
        {
            if (id==null)
            {
                return BadRequest();
            }
            Category category=await _context.Categories.FirstOrDefaultAsync(c=>c.Id==id);
            if (category==null) return BadRequest();
            ViewBag.Category = await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id,Category category, bool? status,int page=1)
        {
            ViewBag.Category = await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();
            Category dbCategory=await _context.Categories.FirstOrDefaultAsync(c=>c.Id==id);
            if (!ModelState.IsValid)
            {
                return View(dbCategory);
            }
            if (id != dbCategory.Id)
            {
                return BadRequest();
            }
            if (string.IsNullOrWhiteSpace(category.Name))
            {
                ModelState.AddModelError("Name", "Category name can't be empty");
                return View(dbCategory);
            }
            if (await _context.Categories.AnyAsync(c=>c.Id!=id&&c.Name.ToLower()==category.Name.ToLower()))
            {
                ModelState.AddModelError("Name","This category name is already taken");
                return View(dbCategory);
            }
            if (dbCategory==null)
            {
                return NotFound();
            }
            dbCategory.Name = category.Name;
            dbCategory.UpdatedAt = DateTime.UtcNow.AddHours(4);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { status=status,page=page});
        }

        public async Task<IActionResult> Delete(int? id,bool?status,int page=1)
        {
            if (id==null)
            {
                return BadRequest();
            }
            Category dbCategory=await _context.Categories.FirstOrDefaultAsync(c=>c.Id==id);
            if (dbCategory==null)
            {
                return NotFound();
            }
            dbCategory.IsDeleted=true;
            dbCategory.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();
            ViewBag.Status=status;

            IEnumerable<Category> categories = await _context.Categories
                .Include(c => c.Products)
                .Where(c => status != null ? c.IsDeleted == status : true)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
            ViewBag.PageIndex=page;
            ViewBag.PageCount = Math.Ceiling((double)categories.Count() / 5);
            return PartialView("_CategoryIndexPartial",categories.Skip((page-1)*5).Take(5));
        }

        public async Task<IActionResult> Restore(int? id, bool? status, int page = 1)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Category dbCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (dbCategory == null)
            {
                return NotFound();
            }
            dbCategory.IsDeleted = false;

            await _context.SaveChangesAsync();
            ViewBag.Status = status;

            IEnumerable<Category> categories = await _context.Categories
                .Include(c => c.Products)
                .Where(c => status != null ? c.IsDeleted == status : true)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)categories.Count() / 5);
            return PartialView("_CategoryIndexPartial", categories.Skip((page - 1) * 5).Take(5));
        }
    }
}
