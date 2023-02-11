using Kontakt.DAL;
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
    public class TagController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public TagController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(bool? status, int page = 1)
        {
            ViewBag.Status = status;

            IQueryable<Tag> tags = _context.Tags
                .Include(x => x.ProductTags).ThenInclude(x=>x.Tag)
                .OrderByDescending(c => c.CreatedAt)
                .AsQueryable();

            if (status != null)
                tags = tags.Where(c => c.IsDeleted == status);

            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)tags.Count() / 5);
            return View(tags.Skip((page - 1) * 5).Take(5).ToList());
        }
        public async Task<IActionResult> Create()
        {
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tag tag, bool? status, int page = 1)
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

            if (await _context.Tags.AnyAsync(t => t.Name.ToLower() == tag.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "Alreade Exists");
                return View();
            }

            
            tag.CreatedAt = DateTime.UtcNow.AddHours(4);
            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { status = status, page = page });
        }

        public async Task<IActionResult> Update(int? id, bool? status, int page = 1)
        {
            ViewBag.Status = status;
            ViewBag.PageIndex = page;
            ViewBag.SubCategory = await _context.Tags.Where(c =>!c.IsDeleted).ToListAsync();
            if (id == null) return BadRequest();

            Tag tag = await _context.Tags
                .Include(c => c.ProductTags).ThenInclude(x=>x.Product)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (tag == null) return NotFound();


            

            return View(tag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Tag tag, bool? status, int page = 1)
        {
            


            Tag dbTag = await _context.Tags
               .Include(c => c.ProductTags).ThenInclude(c => c.Product)
               .FirstOrDefaultAsync(c => c.Id == id);

            if (dbTag == null) return NotFound();




            if (!ModelState.IsValid)
            {
                return View(dbTag);
            }

            if (id != tag.Id) return BadRequest();

            if (string.IsNullOrWhiteSpace(tag.Name))
            {
                ModelState.AddModelError("Name", "Bosluq Olmamalidir");
                return View(dbTag);
            }


            if (await _context.Tags.AnyAsync(t => t.Id != id && t.Name.ToLower() == tag.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "Alreade Exists");
                return View(dbTag);
            }





            dbTag.Name = tag.Name;
            dbTag.UpdatedAt = DateTime.UtcNow.AddHours(4);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { status = status, page = page });
        }

        public async Task<IActionResult> DeleteRestore(int? id, bool? status, int page = 1)
        {
            if (id == null) return BadRequest();

            Tag dbTag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id);

            if (dbTag == null) return NotFound();

            if (dbTag.IsDeleted)
            {
                dbTag.IsDeleted = false;
            }
            else
            {
                dbTag.IsDeleted = true;
                dbTag.DeletedAt = DateTime.UtcNow.AddHours(4);
            }


            await _context.SaveChangesAsync();

            ViewBag.Status = status;

            IQueryable<Tag> tags = _context.Tags
                .Include(x => x.ProductTags).ThenInclude(x => x.Tag)
                .OrderByDescending(c => c.CreatedAt)
                .AsQueryable();

            if (status != null)
                tags = tags.Where(c => c.IsDeleted == status);

            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)tags.Count() / 5);
            return PartialView("_TagIndexPartial", tags.Skip((page - 1) * 5).Take(5));

        }

        public IActionResult PageChange(string key, bool? status, int page = 1)
        {




            IQueryable<Tag> tags = _context.Tags
                .Include(x => x.ProductTags).ThenInclude(x => x.Tag)
                .OrderByDescending(c => c.CreatedAt)
                .AsQueryable();

            if (status != null)
                tags = tags.Where(c => c.IsDeleted == status);

            if (key != null)
            {
                tags = tags.Where(c => c.Name.ToLower().Contains(key.ToLower()));
            }


            ViewBag.Status = status;
            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)tags.Count() / 8);
            return PartialView("_TagIndexPartial", tags.Skip((page - 1) * 8).Take(8));

        }
    }
}
