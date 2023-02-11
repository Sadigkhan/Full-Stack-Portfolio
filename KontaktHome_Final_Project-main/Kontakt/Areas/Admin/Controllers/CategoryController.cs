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
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public CategoryController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(string key, bool? status, bool? TypeStatus, int page = 1)
        {
            ViewBag.Status = status;

            IQueryable<Category> categories = _context.Categories
                .Include(x=>x.Parent)
                .Include(x=>x.Children).ThenInclude(x=>x.Products)
                .Include(x=>x.Products)
                .OrderByDescending(c => c.CreatedAt)
                .AsQueryable();

            if (status != null)
                categories = categories.Where(c => c.IsMain == status);

            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)categories.Count() / 5);
            return View(categories.Skip((page - 1) * 5).Take(5).ToList());
        }
        public async Task<IActionResult>  Create()
        {
            ViewBag.MainCategory = await _context.Categories.Where(c => c.IsMain && !c.IsDeleted).ToListAsync();
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category, string key, bool? status, bool? TypeStatus, int page = 1)
        {
            ViewBag.MainCategory = await _context.Categories.Where(c => c.IsMain && !c.IsDeleted).ToListAsync();

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (string.IsNullOrWhiteSpace(category.Name))
            {
                ModelState.AddModelError("Name", "Bosluq Olmamalidir");
                return View();
            }

            

            

            if (await _context.Categories.AnyAsync(t => t.Name.ToLower() == category.Name.ToLower() && t.ParentId==category.ParentId))
            {
                ModelState.AddModelError("Name", "Alreade Exists");
                return View();
            }

            if (category.IsMain)
            {
                

                if (category.CategoryImage == null)
                {
                    ModelState.AddModelError("CategoryImage", "Sekil Mutleq Olmalidi");
                    return View();
                }

                if (!category.CategoryImage.CheckFileContentType("image/png"))
                {
                    ModelState.AddModelError("CategoryImage", "Secilen Seklin Novu Uygun");
                    return View();
                }

                if (!category.CategoryImage.CheckFileSize(30))
                {
                    ModelState.AddModelError("CategoryImage", "Secilen Seklin Olcusu Maksimum 30 Kb Ola Biler");
                    return View();
                }
                category.ParentId = null;
                category.ImageUrl = category.CategoryImage.CreateFile(_env, "user", "assets","img", "MainCtgImg");
            }
            else
            {
                if (category.CategoryImage == null)
                {
                    ModelState.AddModelError("CategoryImage", "Sekil Mutleq Olmalidi");
                    return View();
                }

                if (!category.CategoryImage.CheckFileContentType("image/jpeg"))
                {
                    ModelState.AddModelError("CategoryImage", "Secilen Seklin Novu Uygun");
                    return View();
                }

                if (!category.CategoryImage.CheckFileSize(300))
                {
                    ModelState.AddModelError("CategoryImage", "Secilen Seklin Olcusu Maksimum 30 Kb Ola Biler");
                    return View();
                }

                category.ImageUrl = category.CategoryImage.CreateFile(_env, "user", "assets", "img", "SubCtgImg");

                if (category.ParentId == null)
                {
                    ModelState.AddModelError("ParentId", "Parent Mutleq Secilmelidir");
                    return View();
                }

                if (!await _context.Categories.AnyAsync(c => c.Id == category.ParentId && !c.IsDeleted))
                {
                    ModelState.AddModelError("ParentId", "Parent Id yanlisdir");
                    return View();
                }
            }

            category.CreatedAt = DateTime.UtcNow.AddHours(4);

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { status = status, TypeStatus = TypeStatus, page = page });
        }

        public async Task<IActionResult> Detail(int? id, string key, bool? status, bool? TypeStatus, int page = 1)
        {
            ViewBag.Status = status;
            ViewBag.PageIndex = page;
            if (id == null) return BadRequest();

            Category category = await _context.Categories
                .Include(c=>c.Products)
                .Include(c=>c.Children)
                .Include(c=>c.CategoryBrands).ThenInclude(c=>c.Brand)
                .Include(c=>c.Parent)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (category == null) return NotFound();

            return View(category);
        }

        public async Task<IActionResult> Update(int? id, string key, bool? status, bool? TypeStatus, int page = 1)
        {
            ViewBag.Status = status;
            ViewBag.PageIndex = page;
            if (id == null) return BadRequest();

            Category category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category == null) return NotFound();

            ViewBag.MainCategory = await _context.Categories.Where(c => c.Id != id && c.IsMain && !c.IsDeleted).ToListAsync();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Category category, string key, bool? status, bool? TypeStatus, int page = 1)
        {
            ViewBag.MainCategory = await _context.Categories.Where(c => c.Id != id && c.IsMain && !c.IsDeleted).ToListAsync();

            Category dbCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (dbCategory == null) return NotFound();

            if (!ModelState.IsValid)
            {
                return View(dbCategory);
            }

            if (id != category.Id) return BadRequest();

            if (string.IsNullOrWhiteSpace(category.Name))
            {
                ModelState.AddModelError("Name", "Bosluq Olmamalidir");
                return View(dbCategory);
            }


            if (await _context.Categories.AnyAsync(t => t.Id != id && t.Name.ToLower() == category.Name.ToLower() &&t.ParentId==category.ParentId))
            {
                ModelState.AddModelError("Name", "Alreade Exists");
                return View(dbCategory);
            }

            if (category.ParentId != null && id == category.ParentId)
            {
                ModelState.AddModelError("ParentId", "Duzun Parent Sec");
                return View(category);
            }

            if (dbCategory.IsMain)
            {
                if (category.IsMain)
                {
                    

                    if (category.CategoryImage != null)
                    {
                        if (!category.CategoryImage.CheckFileContentType("image/png"))
                        {
                            ModelState.AddModelError("CategoryImage", "Secilen Seklin Novu Uygun");
                            return View(dbCategory);
                        }

                        if (!category.CategoryImage.CheckFileSize(300))
                        {
                            ModelState.AddModelError("CategoryImage", "Secilen Seklin Olcusu Maksimum 30 Kb Ola Biler");
                            return View(dbCategory);
                        }

                        if (dbCategory.ImageUrl != null)
                        {
                            Helper.DeleteFile(_env, dbCategory.ImageUrl, "user", "assets", "img", "MainCtgImg");
                        }

                        dbCategory.ImageUrl = category.CategoryImage.CreateFile(_env, "user", "assets", "img", "MainCtgImg");
                    }

                    
                }
                else
                {
                    if (category.ParentId != null)
                    {
                        dbCategory.ParentId = category.ParentId;
                    }
                    else
                    {
                        ModelState.AddModelError("ParentId", "Parent secilmeyib");
                        return View(dbCategory);
                    }
                    if (category.CategoryImage != null)
                    {
                        if (!category.CategoryImage.CheckFileContentType("image/jpeg"))
                        {
                            ModelState.AddModelError("CategoryImage", "Secilen Seklin Novu Uygun");
                            return View(dbCategory);
                        }

                        if (!category.CategoryImage.CheckFileSize(300))
                        {
                            ModelState.AddModelError("CategoryImage", "Secilen Seklin Olcusu Maksimum 30 Kb Ola Biler");
                            return View(dbCategory);
                        }

                        if (dbCategory.ImageUrl != null)
                        {
                            Helper.DeleteFile(_env, dbCategory.ImageUrl, "user", "assets", "img", "MainCtgImg");
                        }

                        dbCategory.ImageUrl = category.CategoryImage.CreateFile(_env, "user", "assets", "img", "SubCtgImg");
                        
                    }
                    else
                    {
                        ModelState.AddModelError("CategoryImage", "Sekil Mutleq secilmelidir");
                        return View(dbCategory);
                    }
                    
                    
                }
            }
            else
            {
                if (category.IsMain)
                {

                    
                    if (category.CategoryImage != null)
                    {
                        if (!category.CategoryImage.CheckFileContentType("image/png"))
                        {
                            ModelState.AddModelError("CategoryImage", "Secilen Seklin Novu Uygun");
                            return View(dbCategory);
                        }

                        if (!category.CategoryImage.CheckFileSize(300))
                        {
                            ModelState.AddModelError("CategoryImage", "Secilen Seklin Olcusu Maksimum 30 Kb Ola Biler");
                            return View(dbCategory);
                        }

                        if (dbCategory.ImageUrl != null)
                        {
                            Helper.DeleteFile(_env, dbCategory.ImageUrl, "user", "assets", "img", "SubCtgImg");
                        }

                        dbCategory.ImageUrl = category.CategoryImage.CreateFile(_env, "user", "assets", "img", "MainCtgImg");
                    }
                    else
                    {
                        ModelState.AddModelError("CategoryImage", "Sekil Mutleq secilmelidir");
                        return View(dbCategory);
                    }

                    dbCategory.ParentId =null;
                }
                else
                {
                    if (category.CategoryImage != null)
                    {
                        if (!category.CategoryImage.CheckFileContentType("image/jpeg"))
                        {
                            ModelState.AddModelError("CategoryImage", "Secilen Seklin Novu Uygun");
                            return View(dbCategory);
                        }

                        if (!category.CategoryImage.CheckFileSize(300))
                        {
                            ModelState.AddModelError("CategoryImage", "Secilen Seklin Olcusu Maksimum 30 Kb Ola Biler");
                            return View(dbCategory);
                        }

                        if (dbCategory.ImageUrl != null)
                        {
                            Helper.DeleteFile(_env, dbCategory.ImageUrl, "user", "assets", "img", "SubCtgImg");
                        }

                        dbCategory.ImageUrl = category.CategoryImage.CreateFile(_env, "user", "assets", "img", "SubCtgImg");
                        
                    }
                    dbCategory.ParentId = category.ParentId;
                }
            }

            dbCategory.IsMain = category.IsMain;
            dbCategory.Name = category.Name;
            dbCategory.UpdatedAt = DateTime.UtcNow.AddHours(4);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { status = status, TypeStatus = TypeStatus, page = page });
        }

        public async Task<IActionResult> DeleteRestore(int? id, string key, bool? status, bool? TypeStatus, int page = 1)
        {
            if (id == null) return BadRequest();

            Category dbCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (dbCategory == null) return NotFound();
            if (!dbCategory.IsDeleted)
            {
                dbCategory.IsDeleted = true;
                dbCategory.DeletedAt = DateTime.UtcNow.AddHours(4);
            }
            else
            {
                dbCategory.IsDeleted = false;
                dbCategory.DeletedAt = null;
            }

            await _context.SaveChangesAsync();

            IQueryable<Category> categories = _context.Categories
                .Include(x => x.Parent)
                .Include(x => x.Children)
                .Include(c => c.Products)
                .OrderByDescending(c => c.CreatedAt)
                .AsQueryable();

            if (status != null)
            {
                categories = categories.Where(c => c.IsDeleted == status);
            }

            if (TypeStatus != null)
            {
                categories = categories.Where(c => c.IsMain == TypeStatus);
            }

            if (key != null)
            {
                categories = categories.Where(c => c.Name.ToLower().Contains(key.ToLower()));
            }
            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)categories.Count() / 5);
            return PartialView("_CategoryIndexPartial", categories.Skip((page - 1) * 5).Take(5));

        }

        public IActionResult PageChange(string key,string keyCtg, bool? status, bool? TypeStatus, int page = 1)
        {


            

            IQueryable<Category> categories =  _context.Categories
                .Include(x => x.Parent)
                .Include(x => x.Children).ThenInclude(x => x.Products)
                .Include(c => c.Products)
                .OrderByDescending(c => c.CreatedAt)
                .AsQueryable();

            if (status != null)
            {
                categories = categories.Where(c => c.IsDeleted == status);
            }

            if (TypeStatus != null)
            {
                categories = categories.Where(c => c.IsMain == TypeStatus);
            }

            if (key!=null)
            {
                categories = categories.Where(c => c.Name.ToLower().Contains(key.ToLower()));
            }
            if (keyCtg != null)
            {
                categories = categories.Where(c => c.Parent.Name.ToLower().Contains(keyCtg.ToLower()));
            }


            ViewBag.Status = status;
            ViewBag.TypeStatus = TypeStatus;
            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)categories.Count() / 5);
            return PartialView("_CategoryIndexPartial", categories.Skip((page - 1) * 5).Take(5));

        }

        public async Task<IActionResult> GetSubCtg(int? id)
        {

            List<Category> categories = await _context.Categories
                .Where(x => x.ParentId == id && !x.IsDeleted).OrderBy(x => x.Id).ToListAsync();

            return PartialView("_GetSubCtgList", categories);

        }
        public async Task<IActionResult> GetParentSubCtg(int? id)
        {

            List<Category> categories = await _context.Categories
                .Where(x => x.ParentId == id && !x.IsDeleted).OrderBy(x => x.Id).ToListAsync();

            return PartialView("_GetParentCtgList", categories);

        }


    }
}
