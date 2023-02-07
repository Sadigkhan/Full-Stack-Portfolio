using DekorEvStartUpFinal.DAL;
using DekorEvStartUpFinal.Extensions;
using DekorEvStartUpFinal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
    public class SliderController : Controller
    {
        public readonly DekorEvStartupAppDbContext _context;
        public readonly IWebHostEnvironment _env;
        public SliderController(DekorEvStartupAppDbContext context,IWebHostEnvironment env)
        {
                _context = context;
                _env = env;
        }
        public async Task<IActionResult> Index(bool?status,int page=1)
        {
            ViewBag.Status = status;

            IEnumerable<Slider> sliders = await _context.Sliders
                .OrderByDescending(s => s.CreatedAt).Where(s => !s.IsDeleted)
                .ToListAsync();

            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)sliders.Count()/5);

            return View(sliders.Skip((page-1)*5).Take(5));
        }

        public async Task<IActionResult> Create(bool? status,int page = 1)
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider,bool?status,int page = 1)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (slider.LeftTopImageFile==null)
            {
                ModelState.AddModelError("LeftTopImageFile", "Slider images are required");
                return View();
            }
            else
            {
                if (!slider.LeftTopImageFile.CheckFileContentType("image/"))
                {
                    ModelState.AddModelError("LeftTopImageFile", "File type must be image");
                    return View();
                }

                if (!slider.LeftTopImageFile.CheckFileSize(100))
                {
                    ModelState.AddModelError("LeftTopImageFile", "File size can't be more than 100 Kb");
                    return View();
                }

                slider.LeftTopImage = slider.LeftTopImageFile.CreateFile(_env, "assets", "images");
            }


            if (slider.LeftBottomImageFile == null)
            {
                ModelState.AddModelError("LeftBottomImageFile", "Slider images are required");
                return View();
            }
            else
            {
                if (!slider.LeftBottomImageFile.CheckFileContentType("image/"))
                {
                    ModelState.AddModelError("LeftBottomImageFile", "File type must be image");
                    return View();
                }

                if (!slider.LeftBottomImageFile.CheckFileSize(100))
                {
                    ModelState.AddModelError("LeftBottomImageFile", "File size can't be more than 100 Kb");
                    return View();
                }

                slider.LeftBottomImage = slider.LeftBottomImageFile.CreateFile(_env, "assets", "images");
            }


            if (slider.RightMainImageFile == null)
            {
                ModelState.AddModelError("RightMainImageFile", "Slider images are required");
                return View();
            }
            else
            {
                if (!slider.RightMainImageFile.CheckFileContentType("image/"))
                {
                    ModelState.AddModelError("RightMainImageFile", "File type must be image");
                    return View();
                }

                if (!slider.RightMainImageFile.CheckFileSize(100))
                {
                    ModelState.AddModelError("RightMainImageFile", "File size can't be more than 100 Kb");
                    return View();
                }

                slider.RightMainImage = slider.RightMainImageFile.CreateFile(_env, "assets", "images");
            }

            slider.CreatedAt = DateTime.UtcNow.AddHours(4);

            await _context.AddAsync(slider);
            await _context.SaveChangesAsync();

            return RedirectToAction("index", new { status, page });
        }

        public async Task<ActionResult> Update(int?id,bool?status,int page=1)
        {
            if (id==null)
            {
                return BadRequest();
            }

            Slider slider = await _context.Sliders.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
            if (slider == null)return NotFound();

            return View(slider);    
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(int?id,Slider slider,bool? status,int page = 1)
        {
            if (id==null)
            {
                return BadRequest();
            }
            if (id != slider.Id)
            {
                return BadRequest();
            }

            Slider dbSlider=await _context.Sliders.FirstOrDefaultAsync(p => p.Id == id&&!p.IsDeleted);

            if (dbSlider==null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(dbSlider);
            }

            if (slider.LeftTopImageFile!=null)
            {
                if (!slider.LeftTopImageFile.CheckFileContentType("image/"))
                {
                    ModelState.AddModelError("LeftTopImageFile", "File type must be image");
                    return View(dbSlider);
                }
                if (!slider.LeftTopImageFile.CheckFileSize(100))
                {
                    ModelState.AddModelError("LeftTopImageFile", "File siz ecan't be more than 100 Kb");
                    return View(dbSlider);
                }
                Helper.Helper.DeleteFile(_env, dbSlider.LeftTopImage, "assets", "images");

                dbSlider.LeftTopImage = slider.LeftTopImageFile.CreateFile(_env, "assets", "images");
            }


            if (slider.LeftBottomImageFile != null)
            {
                if (!slider.LeftBottomImageFile.CheckFileContentType("image/"))
                {
                    ModelState.AddModelError("LeftBottomImageFile", "File type must be image");
                    return View(dbSlider);
                }
                if (!slider.LeftBottomImageFile.CheckFileSize(100))
                {
                    ModelState.AddModelError("LeftBottomImageFile", "File siz ecan't be more than 100 Kb");
                    return View(dbSlider);
                }
                Helper.Helper.DeleteFile(_env, dbSlider.LeftBottomImage, "assets", "images");

                dbSlider.LeftBottomImage = slider.LeftBottomImageFile.CreateFile(_env, "assets", "images");
            }



            if (slider.RightMainImageFile != null)
            {
                if (!slider.RightMainImageFile.CheckFileContentType("image/"))
                {
                    ModelState.AddModelError("RightMainImageFile", "File type must be image");
                    return View(dbSlider);
                }
                if (!slider.RightMainImageFile.CheckFileSize(100))
                {
                    ModelState.AddModelError("RightMainImageFile", "File siz ecan't be more than 100 Kb");
                    return View(dbSlider);
                }
                Helper.Helper.DeleteFile(_env, dbSlider.RightMainImage, "assets", "images");

                dbSlider.RightMainImage = slider.RightMainImageFile.CreateFile(_env, "assets", "images");
            }

            dbSlider.UpdatedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return RedirectToAction("index", new { status,page});
        }
    }
}
