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
    public class ColorController : Controller
    {
        private readonly JuanAppDbContext _context;
        public ColorController(JuanAppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult>  Index(bool?status,int page=1)
        {
            ViewBag.Status = status;
            IEnumerable<Color>Colors=await _context.Colors
                .Include(c=>c.ProductColors).Where(c=>status!=null?c.IsDeleted==status:true)
                .OrderByDescending(c=>c.CreatedAt)
                .ToListAsync();
            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)Colors.Count()/5);
            return View(Colors.Skip((page-1)*5).Take(5));
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(Color color)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (string.IsNullOrWhiteSpace(color.Name))
            {
                ModelState.AddModelError("Name", "Color name filed can't be empty");
                return View();
            }
            if (color.Name.CheckString())
            {
                ModelState.AddModelError("Name", "Only Letters allowed");
                return View();
            }
            if (await _context.Colors.AnyAsync(c=>c.Name.ToLower()==color.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "This color is already exists");
                return View();
            }
            color.CreatedAt = DateTime.UtcNow.AddHours(4);
            await _context.Colors.AddAsync(color);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int? id,bool?status,int page=1)
        {
            if (id==null)
            {
                return BadRequest();
            }
            Color dbColor=await _context.Colors.FirstOrDefaultAsync(c=>c.Id==id);
            if (dbColor == null)
            {
                return NotFound();
            }
            return View(dbColor);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int?id,Color color,bool? status,int page = 1)
        {
            if (!ModelState.IsValid)
            {
                return View(color);
            }
            if (id==null)
            {
                return BadRequest();
            }
            if (id!=color.Id)
            {
                return BadRequest();
            }
            Color dbColor= await _context.Colors.FirstOrDefaultAsync(c=>c.Id==id);
            if (dbColor==null)
            {
                return NotFound();
            }
            if (string.IsNullOrWhiteSpace(color.Name))
            {
                ModelState.AddModelError("Name", "Color name can't be emty");
                return View(dbColor);
            }
            if (color.Name.CheckString())
            {
                ModelState.AddModelError("Name","You can't use whitespace");
                return View(dbColor);
            }
            if (await _context.Colors.AnyAsync(c=>c.Id!=id&&c.Name.ToLower()==color.Name.ToLower()))
            {
                ModelState.AddModelError("Name","This color name already exists");
                return View(dbColor);
            }
            dbColor.Name=color.Name;
            dbColor.UpdatedAt=DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new {status=status,page=page });
        }


        public async Task<IActionResult> Delete(int? id, bool? status, int page = 1)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Color dbColor = await _context.Colors.FirstOrDefaultAsync(c => c.Id == id);
            if (dbColor == null) return NotFound();

            dbColor.IsDeleted = true;
            dbColor.DeletedAt = DateTime.UtcNow.AddHours(4);
            await _context.SaveChangesAsync();
            ViewBag.Status = status;

            IEnumerable<Color> colors = await _context.Colors
                .Include(s => s.ProductColors)
                .Where(s => status != null ? s.IsDeleted == status : true)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)colors.Count() / 5);

            return PartialView("_ColorIndexPartial", colors.Skip((page - 1) * 5).Take(5));
        }


        public async Task<IActionResult> Restore(int? id, bool? status, int page = 1)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Color dbColor = await _context.Colors.FirstOrDefaultAsync(c => c.Id == id);
            if (dbColor == null) return NotFound();

            dbColor.IsDeleted = false;
            dbColor.DeletedAt = null;
            await _context.SaveChangesAsync();
            ViewBag.Status = status;

            IEnumerable<Color> colors = await _context.Colors
                .Include(s => s.ProductColors)
                .Where(s => status != null ? s.IsDeleted == status : true)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)colors.Count() / 5);

            return PartialView("_ColorIndexPartial", colors.Skip((page - 1) * 5).Take(5));
        }
    }
}
