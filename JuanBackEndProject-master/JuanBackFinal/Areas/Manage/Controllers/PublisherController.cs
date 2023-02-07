using DekorEvFinal.Helper;
using JuanBackFinal.DAL;
using JuanBackFinal.Extensions;
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
    public class PublisherController : Controller
    {
        public readonly JuanAppDbContext _context;
        public readonly IWebHostEnvironment _env;
        public PublisherController(JuanAppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index(bool?status,int page=1)
        {

            ViewBag.Status = status;

            IEnumerable<Publisher> publishers = await _context.Publishers
                .Include(p => p.Blogs)
                .Where(b => status != null ? b.IsDeleted == status : !b.IsDeleted)
                .OrderByDescending(b => b.Blogs.Count())
                .ToListAsync();

            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)publishers.Count() / 5);

            return View(publishers.Skip((page-1)*5).Take(5));
        }
        public async Task<IActionResult> Detail(int?id,bool?status,int page=1)
        {
            Publisher publisher=await _context.Publishers.Include(p=>p.Blogs).FirstOrDefaultAsync(p=>p.Id==id);

            if (publisher==null)
            {
                return NotFound();
            }
            return View(publisher);
        }

        public async Task<IActionResult> Create(bool ?status,int page=1)
        {
            ViewBag.Blogs = await _context.Blogs.Where(b => !b.IsDeleted).ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
         public async Task<IActionResult> Create(Publisher publisher,bool? status,int page=1)
        {
            ViewBag.Blogs = await _context.Blogs.Where(b => !b.IsDeleted).ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (publisher.PublisherImageFile==null)
            {
                ModelState.AddModelError("PublisherImageFile", "Publisher image is required");
                return View();
            }
            else
            {
                if (!publisher.PublisherImageFile.CheckFileContentType("image/"))
                {
                    ModelState.AddModelError("PublisherImageFile","File type must be image");
                    return View();
                }

                if (!publisher.PublisherImageFile.CheckFileSize(100))
                {
                    ModelState.AddModelError("PublisherImageFile","File size can't be more than 100 Kb");
                    return View();
                }

                publisher.PublisherImage = publisher.PublisherImageFile.CreateFile(_env, "assets", "img", "blog");

            }
            publisher.CreatedAt = DateTime.UtcNow.AddHours(4);
            await _context.Publishers.AddAsync(publisher);
            await _context.SaveChangesAsync();

            return RedirectToAction("index", new { status,page});
        }

        public async Task<IActionResult> Update(int?id,bool?status,int page = 1)
        {
            ViewBag.Blogs=await _context.Blogs.Where(b => !b.IsDeleted).ToListAsync();

            if (id == null) return BadRequest();

            Publisher publisher = await _context.Publishers.Include(p => p.Blogs).FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
            if (publisher == null) return NotFound();

            return View(publisher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(int?id,Publisher publisher,bool? status,int page=1)
        {
            ViewBag.Blogs = await _context.Blogs.Where(b => !b.IsDeleted).ToListAsync();

            if (id==null)
            {
                return BadRequest();
            }
            if (id!=publisher.Id)
            {
                return BadRequest();
            }

            Publisher dbPublisher = await _context.Publishers.Include(p => p.Blogs).FirstOrDefaultAsync(p=>p.Id==id&&!p.IsDeleted);

            if (dbPublisher==null)
            {
                return NotFound();
            }
            if(!ModelState.IsValid) return View(dbPublisher);

            if (publisher.PublisherImageFile != null)
            {
                if (!publisher.PublisherImageFile.CheckFileContentType("image/"))
                {
                    ModelState.AddModelError("PublisherImageFile","File type must be image");
                    return View(dbPublisher);
                }
                if (!publisher.PublisherImageFile.CheckFileSize(100))
                {
                    ModelState.AddModelError("PublisherImageFile","File siz ecan't be more than 100 Kb");
                    return View(dbPublisher);
                }
                Helper.DeleteFile(_env,dbPublisher.PublisherImage, "assets", "img", "blog");

                dbPublisher.PublisherImage = publisher.PublisherImageFile.CreateFile(_env,"assets","img","blog");
            }
            dbPublisher.PublisherPosition=publisher.PublisherPosition;
            dbPublisher.PublisherName = publisher.PublisherName;
            dbPublisher.UpdatedAt = DateTime.UtcNow.AddHours(4);
            await _context.SaveChangesAsync();

            return RedirectToAction("index", new { status,page});
        }


        public async Task<IActionResult> Delete(int? id, bool? status, int page = 1)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Publisher dbPublisher = await _context.Publishers.FirstOrDefaultAsync(c => c.Id == id);
            if (dbPublisher == null)
            {
                return NotFound();
            }
            dbPublisher.IsDeleted = true;
            dbPublisher.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();
            ViewBag.Status = status;

            IEnumerable<Publisher> puplishers = await _context.Publishers
                .Include(c => c.Blogs)
                .Where(c => status != null ? c.IsDeleted == status : true)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)puplishers.Count() / 5);
            return PartialView("_PublisherIndexPartial", puplishers.Skip((page - 1) * 5).Take(5));
        }



        public async Task<IActionResult> Restore(int? id, bool? status, int page = 1)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Publisher dbPublisher = await _context.Publishers.FirstOrDefaultAsync(c => c.Id == id);
            if (dbPublisher == null)
            {
                return NotFound();
            }
            dbPublisher.IsDeleted = false;

            await _context.SaveChangesAsync();
            ViewBag.Status = status;

            IEnumerable<Publisher> publishers = await _context.Publishers
                .Include(c => c.Blogs)
                .Where(c => status != null ? c.IsDeleted == status : true)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)publishers.Count() / 5);
            return PartialView("_PublisherIndexPartial", publishers.Skip((page - 1) * 5).Take(5));
        }
    }
}
