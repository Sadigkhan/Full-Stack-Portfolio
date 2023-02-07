using DekorEvStartUpFinal.DAL;
using DekorEvStartUpFinal.Extensions;
using DekorEvStartUpFinal.Models;
using Microsoft.AspNetCore.Authorization;
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
    public class MaterialController : Controller
    {
        private readonly DekorEvStartupAppDbContext _context;
        public MaterialController(DekorEvStartupAppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(bool? status, int page = 1)
        {
            ViewBag.Status = status;

            IEnumerable<Material> materials = await _context.Materials
                .Where(c => status != null ? c.IsDeleted == status : true)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();

            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)materials.Count() / 5);

            return View(materials.Skip((page - 1) * 5).Take(5));
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Material material)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (string.IsNullOrWhiteSpace(material.Name))
            {
                ModelState.AddModelError("Name", "Material name filed can't be empty");
                return View();
            }
            if (material.Name.CheckString())
            {
                ModelState.AddModelError("Name", "Only Letters allowed");
                return View();
            }
            if (await _context.Materials.AnyAsync(c => c.Name.ToLower() == material.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "This Material is already exists");
                return View();
            }
            material.CreatedAt = DateTime.UtcNow.AddHours(4);
            await _context.Materials.AddAsync(material);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int? id, bool? status, int page = 1)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Material dbMaterial = await _context.Materials.FirstOrDefaultAsync(c => c.Id == id);
            if (dbMaterial == null)
            {
                return NotFound();
            }
            return View(dbMaterial);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Material material, bool? status, int page = 1)
        {
            if (!ModelState.IsValid)
            {
                return View(material);
            }
            if (id == null)
            {
                return BadRequest();
            }
            if (id != material.Id)
            {
                return BadRequest();
            }
            Material dbMaterial = await _context.Materials.FirstOrDefaultAsync(c => c.Id == id);
            if (dbMaterial == null)
            {
                return NotFound();
            }
            if (string.IsNullOrWhiteSpace(material.Name))
            {
                ModelState.AddModelError("Name", "Material name can't be emty");
                return View(dbMaterial);
            }
            if (material.Name.CheckString())
            {
                ModelState.AddModelError("Name", "You can't use whitespace");
                return View(dbMaterial);
            }
            if (await _context.Materials.AnyAsync(c => c.Id != id && c.Name.ToLower() == material.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "This Material name already exists");
                return View(dbMaterial);
            }
            dbMaterial.Name = material.Name;
            dbMaterial.UpdatedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { status = status, page = page });
        }


        public async Task<IActionResult> Delete(int? id, bool? status, int page = 1)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Material dbMaterial = await _context.Materials.FirstOrDefaultAsync(c => c.Id == id);
            if (dbMaterial == null) return NotFound();

            dbMaterial.IsDeleted = true;
            dbMaterial.DeletedAt = DateTime.UtcNow.AddHours(4);
            await _context.SaveChangesAsync();
            ViewBag.Status = status;

            IEnumerable<Material> materials = await _context.Materials
                .Include(s => s.ProductColorMaterials)
                .Where(s => status != null ? s.IsDeleted == status : true)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)materials.Count() / 5);

            return PartialView("_MaterialIndexPartial", materials.Skip((page - 1) * 5).Take(5));
        }


        public async Task<IActionResult> Restore(int? id, bool? status, int page = 1)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Material dbMaterial = await _context.Materials.FirstOrDefaultAsync(c => c.Id == id);
            if (dbMaterial == null) return NotFound();

            dbMaterial.IsDeleted = false;
            dbMaterial.DeletedAt = null;
            await _context.SaveChangesAsync();
            ViewBag.Status = status;

            IEnumerable<Material> materials = await _context.Materials
                .Include(s => s.ProductColorMaterials)
                .Where(s => status != null ? s.IsDeleted == status : true)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)materials.Count() / 5);

            return PartialView("_MaterialIndexPartial", materials.Skip((page - 1) * 5).Take(5));
        }
    }
}
