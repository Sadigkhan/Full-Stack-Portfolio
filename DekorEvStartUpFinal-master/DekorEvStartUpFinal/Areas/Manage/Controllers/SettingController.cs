using DekorEvStartUpFinal.DAL;
using DekorEvStartUpFinal.Extensions;
using DekorEvStartUpFinal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DekorEvStartUpFinal.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "SuperAdmin")]
    public class SettingController : Controller
    {
        private readonly DekorEvStartupAppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public SettingController(DekorEvStartupAppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Settings.FirstOrDefaultAsync());
        }
        public async Task<IActionResult> Detail()
        {
            return View(await _context.Settings.FirstOrDefaultAsync());
        }
        public async Task<IActionResult> Update()
        {
            return View(await _context.Settings.FirstOrDefaultAsync());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Setting setting)
        {
            Setting dbSetting=await _context.Settings.FirstOrDefaultAsync();
            if (!ModelState.IsValid) return View(dbSetting);
            if (setting.LogoImage!=null)
            {
                if (!setting.LogoImage.CheckFileContentType("image/"))
                {
                    ModelState.AddModelError("LogoImage","Please select image type files");
                    return View(dbSetting);
                }
                if (!setting.LogoImage.CheckFileSize(100))
                {
                    ModelState.AddModelError("LogoImage","Logo file must be less than 100 KB");
                    return View(dbSetting);
                }

                Helper.Helper.DeleteFile(_env, dbSetting.Logo,"assets", "images");
                dbSetting.Logo = setting.LogoImage.CreateFile(_env, "assets", "images");
            }
            if (setting.AboutUsImageFile != null)
            {
                if (!setting.AboutUsImageFile.CheckFileContentType("image/"))
                {
                    ModelState.AddModelError("AboutUsImageFile", "Please select image type files");
                    return View(dbSetting);
                }
                if (!setting.AboutUsImageFile.CheckFileSize(100))
                {
                    ModelState.AddModelError("AboutUsImageFile", "Logo file must be less than 100 KB");
                    return View(dbSetting);
                }

                Helper.Helper.DeleteFile(_env, dbSetting.AboutUsImage,  "assets", "images");
                dbSetting.AboutUsImage = setting.AboutUsImageFile.CreateFile(_env,  "assets", "images");
            }
            dbSetting.Offer = setting.Offer;
            dbSetting.Email = setting.Email;
            dbSetting.Adress = setting.Adress;
            dbSetting.PhoneNumber = setting.PhoneNumber;
            dbSetting.AboutUs = setting.AboutUs;
            dbSetting.UpdatedAt = DateTime.UtcNow.AddHours(4);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
