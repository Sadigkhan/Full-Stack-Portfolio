using Kontakt.DAL;
using Kontakt.Models;
using Kontakt.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Kontakt.Helpers.Helper;

namespace Kontakt.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        public AccountController
            (
                UserManager<AppUser> userManager,
                SignInManager<AppUser> signInManager,
                AppDbContext context,
                RoleManager<IdentityRole> roleManager,
                IWebHostEnvironment env,
                IConfiguration config
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _roleManager = roleManager;
            _env = env;
            _config = config;
        }
        public async Task<IActionResult>  Login()
        {
            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (appUser==null)
            {
                return View();
            }

            else
            {
                if ((await _userManager.GetRolesAsync(appUser))[0] != Roles.Member.ToString())
                {
                    return RedirectToAction("Index", "Users");
                }
                else
                {

                    return RedirectToAction("Index", "Home", new { area = "default" });
                }
                
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM login)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Username or Password is wrong");
                return View();
            }
            if (user.IsDeleted)
            {
                ModelState.AddModelError("", "User Deactived");
                return View();
            }
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "Please Email Confirm");
                return View();
            }
            var signInResult = await _signInManager.PasswordSignInAsync(user, login.Password, login.RememberMe, true);

            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError("", $"User is blocket wait 5 min");
                return View();
            }

            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Username or Password is wrong");
                return View();
            }



            if ((await _userManager.GetRolesAsync(user))[0] != Roles.Member.ToString())
            {
                return RedirectToAction("index", "product", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Index", "Home",new { area = "default" });
            }


        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
