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
    public class SliderController : Controller
    {

        public readonly JuanAppDbContext _context;
        public readonly IWebHostEnvironment _env;
        public SliderController(JuanAppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index(bool? status, int page = 1)
        {
            ViewBag.Status = status;

            IEnumerable<Slider> sliders = await _context.Sliders
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)sliders.Count() / 5);

            return View(sliders.Skip((page - 1) * 5).Take(5));


        }

        public async Task<IActionResult> Create(bool? status, int page = 1)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider, bool? status, int page = 1)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (slider.SliderImageFile == null)
            {
                ModelState.AddModelError("SliderImageFile", "Slider image is required");
                return View();
            }
            else
            {
                if (!slider.SliderImageFile.CheckFileContentType("image/"))
                {
                    ModelState.AddModelError("BannerImageFile", "File type must be image");
                    return View();
                }

                if (!slider.SliderImageFile.CheckFileSize(100))
                {
                    ModelState.AddModelError("SliderImageFile", "File size can't be more than 100 Kb");
                    return View();
                }

                slider.SliderImage = slider.SliderImageFile.CreateFile(_env, "assets", "img", "slider");

            }
            slider.CreatedAt = DateTime.UtcNow.AddHours(4);
            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();

            return RedirectToAction("index", new { status, page });
        }

        public async Task<IActionResult> Update(int? id, bool? status, int page = 1)
        {


            if (id == null) return BadRequest();

            Slider slider = await _context.Sliders.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
            if (slider == null) return NotFound();

            return View(slider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(int? id, Slider slider, bool? status, int page = 1)
        {


            if (id == null)
            {
                return BadRequest();
            }
            if (id != slider.Id)
            {
                return BadRequest();
            }

            Slider dbSlider = await _context.Sliders.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (dbSlider == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid) return View(dbSlider);

            if (slider.SliderImageFile != null)
            {
                if (!slider.SliderImageFile.CheckFileContentType("image/"))
                {
                    ModelState.AddModelError("SliderImageFile", "File type must be image");
                    return View(dbSlider);
                }
                if (!slider.SliderImageFile.CheckFileSize(100))
                {
                    ModelState.AddModelError("SliderImageFile", "File siz ecan't be more than 100 Kb");
                    return View(dbSlider);
                }
                Helper.DeleteFile(_env, dbSlider.SliderImage, "assets", "img", "slider");

                dbSlider.SliderImage = slider.SliderImageFile.CreateFile(_env, "assets", "img", "slider");
            }
            dbSlider.Description = slider.Description;
            dbSlider.Title = slider.Title;
            dbSlider.Subtitle = slider.Subtitle;
            dbSlider.UpdatedAt = DateTime.UtcNow.AddHours(4);
            await _context.SaveChangesAsync();

            return RedirectToAction("index", new { status, page });
        }


        public async Task<IActionResult> Delete(int? id, bool? status, int page = 1)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Slider dbSlider = await _context.Sliders.FirstOrDefaultAsync(c => c.Id == id);
            if (dbSlider == null)
            {
                return NotFound();
            }
            dbSlider.IsDeleted = true;
            dbSlider.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();
            ViewBag.Status = status;

            IEnumerable<Slider> sliders = await _context.Sliders
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)sliders.Count() / 5);
            return PartialView("_SliderIndexPartial", sliders.Skip((page - 1) * 5).Take(5));
        }



        public async Task<IActionResult> Restore(int? id, bool? status, int page = 1)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Slider dbSlider = await _context.Sliders.FirstOrDefaultAsync(c => c.Id == id);
            if (dbSlider == null)
            {
                return NotFound();
            }
            dbSlider.IsDeleted = false;

            await _context.SaveChangesAsync();
            ViewBag.Status = status;

            IEnumerable<Slider> sliders = await _context.Sliders
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)sliders.Count() / 5);
            return PartialView("_SliderIndexPartial", sliders.Skip((page - 1) * 5).Take(5));
        }
    }
}
