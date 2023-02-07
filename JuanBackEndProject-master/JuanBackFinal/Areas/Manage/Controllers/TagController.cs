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
    public class TagController : Controller
    {
        public readonly JuanAppDbContext _context;

        public TagController(JuanAppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(bool?status,int page=1)
        {
            ViewBag.Status = status;
            IEnumerable<Tag> tags = await _context.Tags
                .Include(t => t.BlogTags)
                .Where(t => status != null ? t.IsDeleted == status : true)
                .OrderByDescending(t => t.CreatedAt).ToArrayAsync();

            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)tags.Count() / 5);

            return View(tags.Skip((page - 1) * 5).Take(5));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tag tag)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (string.IsNullOrWhiteSpace(tag.Name))
            {
                ModelState.AddModelError("Name", "Bosluq Olmamalidir");
                return View();
            }

          

            if (tag.Name.CheckString())
            {
                ModelState.AddModelError("Name", "Only letters allowed");
                return View();
            }

            if (await _context.Tags.AnyAsync(t => t.Name.ToLower() == tag.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "This tag name is already exists");
                return View();
            }

            tag.CreatedAt = DateTime.UtcNow.AddHours(4);

            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int? id, bool? status, int page = 1)
        {
            if (id == null) return BadRequest();

            Tag tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id);

            if (tag == null) return NotFound();

            return View(tag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Tag tag, bool? status, int page = 1)
        {
            if (!ModelState.IsValid) return View(tag);

            if (id == null) return BadRequest();

            if (id != tag.Id) return BadRequest();

            Tag dbTag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id);

            if (dbTag == null) return NotFound();

            if (string.IsNullOrWhiteSpace(tag.Name))
            {
                ModelState.AddModelError("Name", "Tag Name Is Required");
                return View(tag);
            }

            if (tag.Name.CheckString())
            {
                ModelState.AddModelError("Name", "Only Letters Allowed");
                return View(tag);
            }

            if (await _context.Tags.AnyAsync(t => t.Id != tag.Id && t.Name.ToLower() == tag.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "This tag name is already exists");
                return View(tag);
            }

            dbTag.Name = tag.Name;
            dbTag.UpdatedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { status = status, page = page });
        }

        public async Task<IActionResult> Delete(int? id, bool? status, int page = 1)
        {
            if (id == null) return BadRequest();

            Tag dbTag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id);

            if (dbTag == null) return NotFound();

            dbTag.IsDeleted = true;
            dbTag.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            ViewBag.Status = status;

            IEnumerable<Tag> tags = await _context.Tags
                .Include(t => t.BlogTags)
                .Where(t => status != null ? t.IsDeleted == status : true)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)tags.Count() / 5);



            return PartialView("_TagIndexPartial", tags.Skip((page - 1) * 5).Take(5));
        }

        public async Task<IActionResult> Restore(int? id, bool? status, int page = 1)
        {
            if (id == null) return BadRequest();

            Tag dbTag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id);

            if (dbTag == null) return NotFound();

            dbTag.IsDeleted = false;
            dbTag.DeletedAt = null;

            await _context.SaveChangesAsync();

            ViewBag.Status = status;

            IEnumerable<Tag> tags = await _context.Tags
                .Include(t => t.BlogTags)
                .Where(t => status != null ? t.IsDeleted == status : true)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)tags.Count() / 5);



            return PartialView("_TagIndexPartial", tags.Skip((page - 1) * 5).Take(5));
        }
    }
}
