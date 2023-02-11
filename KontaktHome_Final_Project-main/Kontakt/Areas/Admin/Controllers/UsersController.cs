using Kontakt.DAL;
using Kontakt.Extensions;
using Kontakt.Helpers;
using Kontakt.Models;
using Kontakt.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Kontakt.Helpers.Helper;

namespace Kontakt.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;
        public UsersController
            (
                UserManager<AppUser> userManager,
                SignInManager<AppUser> signInManager,
                AppDbContext context,
                RoleManager<IdentityRole> roleManager,
                IWebHostEnvironment env
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _roleManager = roleManager;
            _env = env;
        }
        public async Task<IActionResult> Index(bool? status, string isRole, int page = 1)
        {
            ViewBag.Status = status;
            ViewBag.isRole = isRole;
            ViewBag.PageIndex = page;



            IQueryable<AppUser> appUsers = _userManager.Users
                .OrderByDescending(c => c.Id)
                .AsQueryable();

            if (status != null)
                appUsers = appUsers.Where(c => c.IsDeleted == status);
            //if (isRole != null)
            //    appUsers = appUsers.Where(x => Roles);


            ViewBag.PageCount = Math.Ceiling((double)appUsers.Count() / 5);

            List<UserVM> userVMs = new List<UserVM>();

            var users = await appUsers.Skip((page - 1) * 5).Take(5).ToListAsync();

            foreach (var user in users)
            {
                userVMs.Add(new UserVM()
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    Name = user.Name,
                    SurName = user.SurName,
                    IsDeleted = user.IsDeleted,
                    Image = user.Image,
                    PhoneNumber = user.PhoneNumber,
                    Role = (await _userManager.GetRolesAsync(user))[0]
                });
            }



            return View(userVMs);
        }
        public async Task<IActionResult> Detail(string id, bool? status, int page = 1)
        {
            if (id == null) return BadRequest();

            AppUser appUser = await _userManager.FindByIdAsync(id);

            if (appUser == null) return NotFound();



            UserVM userVM = new UserVM
            {
                Id = appUser.Id,
                Email = appUser.Email,
                UserName = appUser.UserName,
                Name = appUser.Name,
                SurName = appUser.SurName,
                IsDeleted = appUser.IsDeleted,
                Image = appUser.Image,
                Role = (await _userManager.GetRolesAsync(appUser))[0],
                ZipCode = appUser.ZipCode,
                Address = appUser.Address,
                City = appUser.City,
                State = appUser.State,
                PhoneNumber = appUser.PhoneNumber,
                Country = appUser.Country
            };

            return View(userVM);
        }
        public async Task<IActionResult> Update(string id, bool? status, int page = 1)
        {
            if (id == null) return BadRequest();

            AppUser appUser = await _userManager.FindByIdAsync(id);

            if (appUser == null) return NotFound();



            UserVM userVM = new UserVM
            {
                Id = appUser.Id,
                Email = appUser.Email,
                UserName = appUser.UserName,
                Name = appUser.Name,
                SurName = appUser.SurName,
                IsDeleted = appUser.IsDeleted,
                Image = appUser.Image,
                Role = (await _userManager.GetRolesAsync(appUser))[0],
                ZipCode = appUser.ZipCode,
                Address = appUser.Address,
                City = appUser.City,
                State = appUser.State,
                PhoneNumber = appUser.PhoneNumber,
                Country = appUser.Country,
                ParentName=appUser.ParentName
            };

            return View(userVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProfilUpdate(UserVM user, bool? status, int page = 1)
        {
            AppUser appUser = await _userManager.FindByNameAsync(user.UserName);

            if (appUser == null) return RedirectToAction("index", "home");
            UserVM userVM = new UserVM
            {
                Id = appUser.Id,
                Email = appUser.Email,
                UserName = appUser.UserName,
                Name = appUser.Name,
                SurName = appUser.SurName,
                IsDeleted = appUser.IsDeleted,
                Image = appUser.Image,
                Role = (await _userManager.GetRolesAsync(appUser))[0],
                ZipCode = appUser.ZipCode,
                Address = appUser.Address,
                City = appUser.City,
                State = appUser.State,
                PhoneNumber = appUser.PhoneNumber,
                Country = appUser.Country,
                ParentName=appUser.ParentName,
                
                
            };

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(" ", "ModelState isValid");
                return View("Update", userVM);
            }

            if (appUser.NormalizedUserName != user.UserName.ToUpper() && await _userManager.Users.AnyAsync(u => u.NormalizedUserName == user.UserName.ToUpper()))
            {
                ModelState.AddModelError("UserName", "UserName Alreade Exist");

                return View("Update", userVM);
            }

            if (appUser.NormalizedEmail != user.Email.ToUpper() && await _userManager.Users.AnyAsync(u => u.NormalizedEmail == user.Email.ToUpper()))
            {
                ModelState.AddModelError("Email", "Email Alreade Exist");
                return View("Update", userVM);
            }
            if (user.ImageFile != null)
            {
                if (!user.ImageFile.CheckFileContentType("image/jpeg"))
                {
                    ModelState.AddModelError("ImageFile", "Secilen Seklin Novu Uygun deyil ancaq png secile biler");
                    return View("Update", userVM);
                }

                if (!user.ImageFile.CheckFileSize(3000))
                {
                    ModelState.AddModelError("ImageFile", "Secilen Seklin Olcusu Maksimum 300 Kb Ola Biler");
                    return View("Update", userVM);
                }
                if (appUser.Image != null)
                {
                    Helper.DeleteFile(_env, appUser.Image, "user", "assets", "img", "userimg");
                }


                appUser.Image = user.ImageFile.CreateFile(_env, "user", "assets", "img", "userimg");
            }

            appUser.Name = user.Name;
            appUser.SurName = user.SurName;
            appUser.UserName = user.UserName;
            appUser.Email = user.Email;
            appUser.Address = user.Address;
            appUser.Country = user.Country;
            appUser.City = user.City;
            appUser.State = user.State;
            appUser.ZipCode = user.ZipCode;
            appUser.PhoneNumber = user.PhoneNumber;
            appUser.ParentName = user.ParentName;
            appUser.IsDeleted = user.IsDeleted;
            if (user.Role!="Admin")
            {
                appUser.isAdmin = false;
            }
            else
            {
                appUser.isAdmin = true;
            }

            //if (user.Role.ToString()=="Admin" || user.Role.ToString() == "Member" || user.Role.ToString() == "Maneger" || user.Role.ToString() == "Publisher")
            //{
            //    await _userManager.RemoveFromRoleAsync(appUser, userVM.Role.ToString());
            //    await _userManager.AddToRoleAsync(appUser, user.Role.ToString());
            //}
            foreach (Roles roles in (Roles[])Enum.GetValues(typeof(Roles)))
            {
                if (user.Role.ToString() == roles.ToString())
                {
                    await _userManager.RemoveFromRoleAsync(appUser, userVM.Role.ToString());
                    await _userManager.AddToRoleAsync(appUser, user.Role.ToString());
                }

            }




            IdentityResult identityResult = await _userManager.UpdateAsync(appUser);

            if (!identityResult.Succeeded)
            {
                foreach (var item in identityResult.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View("Update", userVM);
            }

            if (user.Password != null)
            {
                if (string.IsNullOrWhiteSpace(user.CurrentPassword))
                {
                    ModelState.AddModelError("CurrentPassword", "CurrentPassword Is Requered");
                    return View("Update", userVM);
                }

                if (!await _userManager.CheckPasswordAsync(appUser, user.CurrentPassword))
                {
                    ModelState.AddModelError("CurrentPassword", "CurrentPassword Is InCorrect");
                    return View("Update", userVM);
                }

                string token = await _userManager.GeneratePasswordResetTokenAsync(appUser);
                identityResult = await _userManager.ResetPasswordAsync(appUser, token, user.Password);
                if (!identityResult.Succeeded)
                {
                    foreach (var item in identityResult.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                    return View("Update", userVM);
                }
            }

            return RedirectToAction("index", new { status, page });
        }

       
    }
}
