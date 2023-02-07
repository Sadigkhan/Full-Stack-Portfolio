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
    public class BannerController : Controller
    {

        public readonly JuanAppDbContext _context;
        public readonly IWebHostEnvironment _env;
        public BannerController(JuanAppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult>  Index(bool? status,int page=1)
        {
            ViewBag.Status = status;

            IEnumerable<Banner> banners = await _context.Banners
                .OrderByDescending(b => b.CreatedAt).Where(b=>!b.IsDeleted)
                .ToListAsync();

            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)banners.Count() / 5);

            return View(banners.Skip((page - 1) * 5).Take(5));

           
        }

        public async Task<IActionResult> Create(bool? status, int page= 1)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Banner banner,bool? status, int page=1)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (banner.BannerImageFile == null)
            {
                ModelState.AddModelError("BannerImageFile", "Banner image is required");
                return View();
            }
            else
            {
                if (!banner.BannerImageFile.CheckFileContentType("image/"))
                {
                    ModelState.AddModelError("BannerImageFile", "File type must be image");
                    return View();
                }

                if (!banner.BannerImageFile.CheckFileSize(100))
                {
                    ModelState.AddModelError("BannerImageFile", "File size can't be more than 100 Kb");
                    return View();
                }

                banner.BannerImage = banner.BannerImageFile.CreateFile(_env, "assets", "img", "banner");

            }
            banner.CreatedAt = DateTime.UtcNow.AddHours(4);
            await _context.Banners.AddAsync(banner);
            await _context.SaveChangesAsync();

            return RedirectToAction("index", new { status, page });
        }

        public async Task<IActionResult> Update(int? id, bool? status, int page = 1)
        {
           

            if (id == null) return BadRequest();

            Banner banner = await _context.Banners.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
            if (banner == null) return NotFound();

            return View(banner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(int? id, Banner banner, bool? status, int page = 1)
        {
           

            if (id == null)
            {
                return BadRequest();
            }
            if (id != banner.Id)
            {
                return BadRequest();
            }

            Banner dbBanner = await _context.Banners.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (dbBanner == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid) return View(dbBanner);

            if (banner.BannerImageFile != null)
            {
                if (!banner.BannerImageFile.CheckFileContentType("image/"))
                {
                    ModelState.AddModelError("BannerImageFile", "File type must be image");
                    return View(dbBanner);
                }
                if (!banner.BannerImageFile.CheckFileSize(100))
                {
                    ModelState.AddModelError("BannerImageFile", "File siz ecan't be more than 100 Kb");
                    return View(dbBanner);
                }
                Helper.DeleteFile(_env, dbBanner.BannerImage, "assets", "img", "banner");

                dbBanner.BannerImage = banner.BannerImageFile.CreateFile(_env, "assets", "img", "banner");
            }
            dbBanner.SubTitle = banner.SubTitle;
            dbBanner.Title = banner.Title;
            dbBanner.UpdatedAt = DateTime.UtcNow.AddHours(4);
            await _context.SaveChangesAsync();

            return RedirectToAction("index", new { status, page });
        }


        public async Task<IActionResult> Delete(int? id, bool? status, int page = 1)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Banner dbBanner = await _context.Banners.FirstOrDefaultAsync(c => c.Id == id);
            if (dbBanner == null)
            {
                return NotFound();
            }
            dbBanner.IsDeleted = true;
            dbBanner.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();
            ViewBag.Status = status;

            IEnumerable<Banner> banners = await _context.Banners
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)banners.Count() / 5);
            return PartialView("_BannerIndexPartial", banners.Skip((page - 1) * 5).Take(5));
        }



        public async Task<IActionResult> Restore(int? id, bool? status, int page = 1)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Banner dbBanner = await _context.Banners.FirstOrDefaultAsync(c => c.Id == id);
            if (dbBanner == null)
            {
                return NotFound();
            }
            dbBanner.IsDeleted = false;

            await _context.SaveChangesAsync();
            ViewBag.Status = status;

            IEnumerable<Banner> banners = await _context.Banners
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)banners.Count() / 5);
            return PartialView("_BannerIndexPartial", banners.Skip((page - 1) * 5).Take(5));
        }
    }
}

