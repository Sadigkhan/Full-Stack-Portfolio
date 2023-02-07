using DekorEvFinal.Helper;
using JuanBackFinal.DAL;
using JuanBackFinal.Extensions;
using JuanBackFinal.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JuanBackFinal.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SettingController : Controller
    {
        private readonly JuanAppDbContext _context;
        public readonly IWebHostEnvironment _env;

        public SettingController(JuanAppDbContext context, IWebHostEnvironment env)
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
            Setting dbSetting= await _context.Settings.FirstOrDefaultAsync(); 
            if (!ModelState.IsValid) return View(dbSetting);
            if (setting.LogoFile != null)
            {
                if (!setting.LogoFile.CheckFileContentType("image/"))
                {
                    ModelState.AddModelError("LogoFile","Please add image type file");
                    return View(dbSetting);
                }
                if (!setting.LogoFile.CheckFileSize(100))
                {
                    ModelState.AddModelError("LogoFile", "File size can't be more than 100Kb");
                    return View(dbSetting);
                }
                Helper.DeleteFile(_env, dbSetting.Logo, "assets", "img", "logo");
                dbSetting.Logo = setting.LogoFile.CreateFile(_env, "assets", "img", "logo");

            }
            
            dbSetting.WorkHours = setting.WorkHours;
            dbSetting.PhoneNumber = setting.PhoneNumber;
            dbSetting.Email = setting.Email;
            dbSetting.Adress = setting.Adress;
            dbSetting.ContactUsDescription = setting.ContactUsDescription;
            dbSetting.ContactUsTitle = setting.ContactUsTitle;
            dbSetting.Greeting = setting.Greeting;
            dbSetting.UpdatedAt = DateTime.UtcNow.AddHours(4);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
