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
    public class ServiceOfferController : Controller
    {

        public readonly JuanAppDbContext _context;
        public readonly IWebHostEnvironment _env;
        public ServiceOfferController(JuanAppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index(bool? status, int page = 1)
        {
            ViewBag.Status = status;

            IEnumerable<ServiceOffer> serviceOffers = await _context.ServiceOffers
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)serviceOffers.Count() / 5);

            return View(serviceOffers.Skip((page - 1) * 5).Take(5));


        }

        public async Task<IActionResult> Create(bool? status, int page = 1)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceOffer serviceOffer, bool? status, int page = 1)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (serviceOffer.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Service Offer image is required");
                return View();
            }
            else
            {
                if (!serviceOffer.ImageFile.CheckFileContentType("image/"))
                {
                    ModelState.AddModelError("ImageFile", "File type must be image");
                    return View();
                }

                if (!serviceOffer.ImageFile.CheckFileSize(100))
                {
                    ModelState.AddModelError("ImageFile", "File size can't be more than 100 Kb");
                    return View();
                }

                serviceOffer.Image = serviceOffer.ImageFile.CreateFile(_env, "assets", "img", "banner");

            }
            serviceOffer.CreatedAt = DateTime.UtcNow.AddHours(4);
            await _context.ServiceOffers.AddAsync(serviceOffer);
            await _context.SaveChangesAsync();

            return RedirectToAction("index", new { status, page });
        }

        public async Task<IActionResult> Update(int? id, bool? status, int page = 1)
        {


            if (id == null) return BadRequest();

            ServiceOffer serviceOffer = await _context.ServiceOffers.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
            if (serviceOffer == null) return NotFound();

            return View(serviceOffer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(int? id, ServiceOffer serviceOffer, bool? status, int page = 1)
        {


            if (id == null)
            {
                return BadRequest();
            }
            if (id != serviceOffer.Id)
            {
                return BadRequest();
            }

            ServiceOffer dbServiceOffer = await _context.ServiceOffers.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (dbServiceOffer == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid) return View(dbServiceOffer);

            if (serviceOffer.ImageFile != null)
            {
                if (!serviceOffer.ImageFile.CheckFileContentType("image/"))
                {
                    ModelState.AddModelError("ImageFile", "File type must be image");
                    return View(dbServiceOffer);
                }
                if (!serviceOffer.ImageFile.CheckFileSize(100))
                {
                    ModelState.AddModelError("ImageFile", "File siz ecan't be more than 100 Kb");
                    return View(dbServiceOffer);
                }
                Helper.DeleteFile(_env, dbServiceOffer.Image, "assets", "img", "banner");

                dbServiceOffer.Image = serviceOffer.ImageFile.CreateFile(_env, "assets", "img", "banner");
            }
            dbServiceOffer.Description = serviceOffer.Description;
            dbServiceOffer.Title = serviceOffer.Title;
            dbServiceOffer.UpdatedAt = DateTime.UtcNow.AddHours(4);
            await _context.SaveChangesAsync();

            return RedirectToAction("index", new { status, page });
        }


        public async Task<IActionResult> Delete(int? id, bool? status, int page = 1)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ServiceOffer dbServiceOffer = await _context.ServiceOffers.FirstOrDefaultAsync(c => c.Id == id);
            if (dbServiceOffer == null)
            {
                return NotFound();
            }
            dbServiceOffer.IsDeleted = true;
            dbServiceOffer.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();
            ViewBag.Status = status;

            IEnumerable<ServiceOffer> serviceOffers = await _context.ServiceOffers
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)serviceOffers.Count() / 5);
            return PartialView("_ServiceOfferIndexPartial", serviceOffers.Skip((page - 1) * 5).Take(5));
        }



        public async Task<IActionResult> Restore(int? id, bool? status, int page = 1)
        {
            if (id == null)
            {
                return BadRequest();
            }
            ServiceOffer dbServiceOffer = await _context.ServiceOffers.FirstOrDefaultAsync(c => c.Id == id);
            if (dbServiceOffer == null)
            {
                return NotFound();
            }
            dbServiceOffer.IsDeleted = false;

            await _context.SaveChangesAsync();
            ViewBag.Status = status;

            IEnumerable<ServiceOffer> serviceOffers = await _context.ServiceOffers
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)serviceOffers.Count() / 5);
            return PartialView("_ServiceOfferIndexPartial", serviceOffers.Skip((page - 1) * 5).Take(5));
        }
    }
}
