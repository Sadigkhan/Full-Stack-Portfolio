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
    public class SizeController : Controller
    {
        private readonly JuanAppDbContext _context;
        public SizeController(JuanAppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(bool? status, int page = 1)
        {
            ViewBag.Status = status;
            IEnumerable<Size> Sizes = await _context.Sizes
                .Include(c => c.ProductSizes).Where(c => status != null ? c.IsDeleted == status : true)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)Sizes.Count() / 5);
            return View(Sizes.Skip((page - 1) * 5).Take(5));
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(Size size)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (string.IsNullOrWhiteSpace(size.Name))
            {
                ModelState.AddModelError("Name", "Size  filed can't be empty");
                return View();
            }
            if (size.Name.CheckInt())
            {
                ModelState.AddModelError("Name", "You can't use letter");
                return View();
            }
            if (await _context.Sizes.AnyAsync(c => c.Name.ToLower() == size.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "This size is already exists");
                return View();
            }
            size.CreatedAt = DateTime.UtcNow.AddHours(4);
            await _context.Sizes.AddAsync(size);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int? id, bool? status, int page = 1)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Size dbSize = await _context.Sizes.FirstOrDefaultAsync(c => c.Id == id);
            if (dbSize == null)
            {
                return NotFound();
            }
            return View(dbSize);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Size size, bool? status, int page = 1)
        {
            if (!ModelState.IsValid)
            {
                return View(size);
            }
            if (id == null)
            {
                return BadRequest();
            }
            if (id != size.Id)
            {
                return BadRequest();
            }
            Size dbSize = await _context.Sizes.FirstOrDefaultAsync(c => c.Id == id);
            if (dbSize == null)
            {
                return NotFound();
            }
            if (string.IsNullOrWhiteSpace(size.Name))
            {
                ModelState.AddModelError("Name", "Size field can't be emty");
                return View(dbSize);
            }
            if (size.Name.CheckInt())
            {
                ModelState.AddModelError("Name", "Size must be number");
                return View(dbSize);
            }

            if (await _context.Sizes.AnyAsync(c => c.Id != id && c.Name.ToLower() == size.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "This size  already exists");
                return View(dbSize);
            }
            dbSize.Name = size.Name;
            dbSize.UpdatedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { status = status, page = page });
        }
        public async Task<IActionResult> Delete(int?id,bool? status,int page = 1)
        {
            if (id==null)
            {
                return BadRequest();
            }
            Size dbSize= await _context.Sizes.FirstOrDefaultAsync(c => c.Id == id);
            if (dbSize == null) return NotFound();

            dbSize.IsDeleted=true;
            dbSize.DeletedAt = DateTime.UtcNow.AddHours(4);
            await _context.SaveChangesAsync();
            ViewBag.Status = status;

            IEnumerable<Size> sizes = await _context.Sizes
                .Include(s=>s.ProductSizes)
                .Where(s=>status!=null?s.IsDeleted==status:true)
                .OrderByDescending(s=>s.CreatedAt)
                .ToListAsync();
            ViewBag.PageIndex=page;
            ViewBag.PageCount = Math.Ceiling((double)sizes.Count() / 5);

            return PartialView("_SizeIndexPartial", sizes.Skip((page - 1) * 5).Take(5));
        }


        public async Task<IActionResult> Restore(int? id, bool? status, int page = 1)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Size dbSize = await _context.Sizes.FirstOrDefaultAsync(c => c.Id == id);
            if (dbSize == null) return NotFound();

            dbSize.IsDeleted = false;
            dbSize.DeletedAt = null;
            await _context.SaveChangesAsync();
            ViewBag.Status = status;

            IEnumerable<Size> sizes = await _context.Sizes
                .Include(s => s.ProductSizes)
                .Where(s => status != null ? s.IsDeleted == status : true)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)sizes.Count() / 5);

            return PartialView("_SizeIndexPartial", sizes.Skip((page-1)*5).Take(5));
        }
    }
}
