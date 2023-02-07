using DekorEvStartUpFinal.DAL;
using DekorEvStartUpFinal.Extensions;
using DekorEvStartUpFinal.Models;
using DekorEvStartUpFinal.ViewModels.Account;
using DekorEvStartUpFinal.ViewModels.Basket;
using DekorEvStartUpFinal.ViewModels.Compare;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace DekorEvStartUpFinal.Controllers
{
    public class AccountController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly DekorEvStartupAppDbContext _context;
        public readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;

        public AccountController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, DekorEvStartupAppDbContext context, IConfiguration config, IWebHostEnvironment env)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _config = config;
            _env = env;
        }

        public IActionResult Register()
        {
            return View();
        }


        public IActionResult RegisterAsStore()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("index", "home");
            }
            ViewBag.IsMarket = true;
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAsStore(RegisterVM registerVM)
        {
            ViewBag.IsMarket = true;

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("index", "home");
            }

            if (string.IsNullOrWhiteSpace(registerVM.Description))
            {
                
                ModelState.AddModelError("Description","Mağaza haqqında məlumatları daxil edin");
                return View();
            }
            
            AppUser appUser = new AppUser
            {
                FullName = registerVM.FullName,
                UserName = registerVM.UserName,
                Email = registerVM.Email,
                Description = registerVM.Description,
                isAdmin = false,
                isMarket = true,
                isConfirmed = false
            };

            if (registerVM.UserImageFile == null)
            {
                ModelState.AddModelError("UserImageFile", "Mağaza şəklini yükləməyiniz tələb olunur");
                return View();
            }
            if (!registerVM.UserImageFile.CheckFileContentType("image/"))
            {
                ModelState.AddModelError("UserImageFile", "Seçilmiş faylın tipi şəkil olmalıdır");
                return View();
            }
            if (!registerVM.UserImageFile.CheckFileSize(100))
            {
                ModelState.AddModelError("UserImageFile", "Secilmiş şəklin həcmi 100KB-dan artıq ola bilməz");
                return View();
            }
            appUser.UserImageFile = registerVM.UserImageFile;
            appUser.UserImage = appUser.UserImageFile.CreateFile(_env, "assets", "images");

            if (!registerVM.Terms)
            {
                ModelState.AddModelError("Terms", "Sozlesmeni qebul edin");
                return View();
            }

            string token = Guid.NewGuid().ToString();
            appUser.EmailConfirmationToken = token;



            IdentityResult identityResult = await _userManager.CreateAsync(appUser, registerVM.Password);

            if (!identityResult.Succeeded)
            {
                foreach (var item in identityResult.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }
            await _userManager.AddToRoleAsync(appUser, "Market");

            var link = Url.Action(nameof(VerifyEmail), "Account", new { id = appUser.Id, token }, Request.Scheme, Request.Host.ToString());

            EmailVM email = _config.GetSection("Email").Get<EmailVM>();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(email.SenderEmail, email.SenderName);
            mail.To.Add(appUser.Email);
            mail.Subject = "VerifyEmail";
            string body = "";
            using (StreamReader reader = new StreamReader("wwwroot/Assets/Template/verifyemail.html"))
            {
                body = reader.ReadToEnd();
            }
            body=body.Replace("{link}",link);
            //mail.Body = $"<a href=\"{link}\">Verify</a>";
            mail.Body = body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = email.Server;
            smtp.Port = email.Port;
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential(email.SenderEmail, email.Password);
            smtp.Send(mail);

            return RedirectToAction(nameof(EmailVerification));
        }



        public IActionResult RegisterAsMember()
        {
            ViewBag.IsMarket = false;
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAsMember(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("index", "home");
            }
            AppUser appUser = new AppUser
            {
                FullName = registerVM.FullName,
                UserName = registerVM.UserName,
                Email = registerVM.Email,
                isMarket = false,
                isAdmin = false,
                isConfirmed = true
            };

            if (!registerVM.Terms)
            {
                ModelState.AddModelError("Terms", "Sozlesmeni qebul edin");
                return View();
            }

            string token = Guid.NewGuid().ToString();
            appUser.EmailConfirmationToken = token;



            IdentityResult identityResult = await _userManager.CreateAsync(appUser, registerVM.Password);

            if (!identityResult.Succeeded)
            {
                foreach (var item in identityResult.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }
            await _userManager.AddToRoleAsync(appUser, "Member");

            var link = Url.Action(nameof(VerifyEmail), "Account", new { id = appUser.Id, token }, Request.Scheme, Request.Host.ToString());

            EmailVM email = _config.GetSection("Email").Get<EmailVM>();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(email.SenderEmail, email.SenderName);
            mail.To.Add(appUser.Email);
            mail.Subject = "VerifyEmail";
            string body = "";
            using (StreamReader reader = new StreamReader("wwwroot/Assets/Template/verifyemail.html"))
            {
                body = reader.ReadToEnd();
            }
            body=body.Replace("{link}",link);
            //mail.Body = $"<a href=\"{link}\">Verify</a>";
            mail.Body = body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = email.Server;
            smtp.Port = email.Port;
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential(email.SenderEmail, email.Password);
            smtp.Send(mail);
            //await _signInManager.SignInAsync(appUser, isPersistent:false);

            return RedirectToAction(nameof(EmailVerification));
        }



        public IActionResult EmailVerification() => View();



        public async Task<IActionResult> VerifyEmail(string id, string token)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            if (user.EmailConfirmationToken != token)
            {
                return BadRequest();
            }

            var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            IdentityResult result = await _userManager.ConfirmEmailAsync(user, emailConfirmationToken);

            if (result.Succeeded)
            {
                string newToken = Guid.NewGuid().ToString();
                user.EmailConfirmationToken = newToken;
                await _userManager.UpdateAsync(user);
                return View();
            }



            return BadRequest();
        }



        public IActionResult ResetPassword()=>View();



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM reset)
        {
            if (string.IsNullOrWhiteSpace(reset.Email))
            {
                ModelState.AddModelError(string.Empty, "E-poçt ünvanını daxil edin");
                return View();
            }
            AppUser user = await _userManager.FindByEmailAsync(reset.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Daxil etdiyiniz hesab movcud deyil");
                return View();
            }
            var link = Url.Action(nameof(NewPassword), "Account", new { id = user.Id, token = user.PasswordResetToken }, Request.Scheme, Request.Host.ToString());
            EmailVM email = _config.GetSection("Email").Get<EmailVM>();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(email.SenderEmail, email.SenderName);
            mail.To.Add(reset.Email);
            mail.Subject = "Reset Password";
            mail.Body = $"<a href=\"{link}\">Reset Password</a>";
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = email.Server;
            smtp.Port = email.Port;
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential(email.SenderEmail, email.Password);
            smtp.Send(mail);

            return RedirectToAction(nameof(EmailVerification));
        }



        public IActionResult NewPassword(ResetPasswordVM reset)
        {
            return View(reset);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("NewPassword")]
        public async Task<IActionResult> NewPasswordPost(ResetPasswordVM reset)
        {
            if (!ModelState.IsValid)
            {
                return View(reset);
            }
            if (reset.Id == null)
            {
                return NotFound();
            }
            AppUser user = await _userManager.FindByIdAsync(reset.Id);
            if (user == null)
            {
                return NotFound();
            }
            if (user.PasswordResetToken != reset.Token)
            {
                return BadRequest();
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            IdentityResult result = await _userManager.ResetPasswordAsync(user, resetToken, reset.Password);



            if (result.Succeeded)
            {
                string passwordResetToken = Guid.NewGuid().ToString();
                user.PasswordResetToken = passwordResetToken;
                await _userManager.UpdateAsync(user);
                return RedirectToAction("Login");
            }
            return BadRequest();
        }



        public IActionResult Login()
        {

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("index", "home");
            }


            return View();
        }



        public IActionResult ConfirmByAdmin() => View();


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == loginVM.Email.ToUpperInvariant() && !u.isAdmin);

            if (appUser == null)
            {
                ModelState.AddModelError("", "E-poçt və ya şifrəniz yanlışdır");
                return View();
            }



            if (!await _userManager.IsEmailConfirmedAsync(appUser))
            {
                ModelState.AddModelError("", "Zəhmət olmasa ilk öncə hesabınızı təsdiqləyin!");
                return View();
            }

            if (appUser.isConfirmed==false)
            {
                return RedirectToAction(nameof(ConfirmByAdmin));
            }

            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(appUser, loginVM.Password, loginVM.RememberMe, true);

            if (!signInResult.Succeeded)
            {
                if (signInResult.IsLockedOut)
                {
                    ModelState.AddModelError("", "Hesabınız 10 dəqiqə muddətinə bloklanıb");
                    return View();
                }
                ModelState.AddModelError("", "E-poçt və ya şifrəniz yanlışdır");
                
                return View();
            }


            string coockieBasket = HttpContext.Request.Cookies["basket"];

            if (!string.IsNullOrWhiteSpace(coockieBasket))
            {
                List<BasketVM> basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(coockieBasket);

                List<Basket> baskets = new List<Basket>();
                List<Basket> existedBasket = await _context.Baskets.Where(b => b.AppUserId == appUser.Id).ToListAsync();
                foreach (BasketVM basketVM in basketVMs)
                {
                    if (existedBasket.Any(b => b.ProductId == basketVM.ProductId && b.ColorId == basketVM.ColorId && b.MaterialId == basketVM.MaterialId))
                    {
                        existedBasket.Find(b => b.ProductId == basketVM.ProductId && b.ColorId == basketVM.ColorId && b.MaterialId == basketVM.MaterialId).Count = basketVM.Count;
                    }
                    else
                    {
                        Basket basket = new Basket
                        {
                            AppUserId = appUser.Id,
                            ProductId = basketVM.ProductId,
                            Count = basketVM.Count,
                            ColorId= basketVM.ColorId,
                            MaterialId= basketVM.MaterialId,
                            CreatedAt = DateTime.UtcNow.AddHours(4)
                        };

                        baskets.Add(basket);
                    }


                }

                if (baskets.Count > 0)
                {
                    await _context.Baskets.AddRangeAsync(baskets);
                    await _context.SaveChangesAsync();
                }
            }
 

            return RedirectToAction("index", "home");

        }



        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("index", "home");
        }



        [Authorize(Roles ="SuperAdmin,Market,Member")]
        public async Task<IActionResult> Profile()
        {
            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u=>u.NormalizedUserName==User.Identity.Name.ToUpperInvariant()&&!u.isAdmin);
            if (appUser==null)
            {
                return RedirectToAction("index","home");
            }

            MemberProfileVM memberProfileVM = new MemberProfileVM
            {
                Member = new MemberUpdateVM
                {
                    Adress=appUser.Adress,
                    City=appUser.City,
                    FullName=appUser.FullName,
                    PhoneNumber=appUser.PhoneNumber,
                    UserName=appUser.UserName,
                    Email=appUser.Email,
                    UserImage=appUser.UserImage,
                    Description=appUser.Description,
                    
                    
                }
            };
            return View(memberProfileVM);
        }



        [Authorize(Roles = "SuperAdmin,Market,Member")]
        [HttpPost]
        public async Task<IActionResult> Edit(MemberUpdateVM member)
        {
            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u=>u.UserName==User.Identity.Name&&!u.isAdmin);
            member.UserImage = appUser.UserImage;

            if (appUser==null)
            {
                return RedirectToAction("index","home");
            }

            MemberProfileVM memberProfileVM = new MemberProfileVM
            {
                Member = member
            };


            if (!ModelState.IsValid)
            {
                return View("Profile", memberProfileVM);
            }

            if (appUser.NormalizedUserName!=member.UserName.ToUpperInvariant()&& await _userManager.Users.AnyAsync(u=>u.NormalizedUserName==member.UserName.ToUpperInvariant()))
            {
                ModelState.AddModelError("UserName","Daxil etdiyiniz istifadəçi adı artıq mövcuddur");
                return View("Profile", memberProfileVM);
            }
            if (appUser.NormalizedEmail != member.Email.ToUpperInvariant() && await _userManager.Users.AnyAsync(u => u.NormalizedEmail == member.Email.ToUpperInvariant()))
            {
                ModelState.AddModelError("Email", "Daxil etdiyiniz e-poçt ünvanı artıq mövcuddur");
                return View("Profile", memberProfileVM);
            }
            if (appUser.isMarket==true)
            {

                if (member.UserImageFile != null)
                {
                    if (!member.UserImageFile.CheckFileContentType("image/"))
                    {
                        ModelState.AddModelError("UserImageFile", "Seçilmiş faylın tipi şəkil olmalıdır");
                        return View("Profile", memberProfileVM);
                    }
                    if (!member.UserImageFile.CheckFileSize(100))
                    {
                        ModelState.AddModelError("UserImageFile", "Seçilmiş  şəkilin həcmi 100KB-dan çox ola bilməz");
                        return View("Profile", memberProfileVM);
                    }
                    if (appUser.UserImage != null)
                    {
                        Helper.Helper.DeleteFile(_env, appUser.UserImage, "assets", "images");
                    }
                    appUser.UserImage = member.UserImageFile.CreateFile(_env, "assets", "images");
                }

                if (!string.IsNullOrWhiteSpace(member.Description))
                {
                    appUser.Description = member.Description;
                }
                else
                {
                    //member.Description = appUser.Description;

                    ModelState.AddModelError("Description","Mağaza məlumatlarını daxil edin");
                    return View("Profile",memberProfileVM);
                }


                if (!string.IsNullOrWhiteSpace(member.City))
                {
                    appUser.City = member.City;
                }
                else
                {
                    ModelState.AddModelError("City", "Məlumatı doldurmanız tələb olunur");
                    return View("Profile", memberProfileVM);
                }

                if (!string.IsNullOrWhiteSpace(member.Adress))
                {
                    appUser.Adress = member.Adress;
                }
                else
                {
                    ModelState.AddModelError("Adress", "Məlumatı doldurmanız tələb olunur");
                    return View("Profile", memberProfileVM);
                }


            }
            else
            {
                if (member.UserImageFile != null)
                {
                    if (!member.UserImageFile.CheckFileContentType("image/"))
                    {
                        ModelState.AddModelError("UserImageFile", "Seçilmiş faylın tipi şəkil olmalıdır");
                        return View("Profile", memberProfileVM);
                    }
                    if (!member.UserImageFile.CheckFileSize(100))
                    {
                        ModelState.AddModelError("UserImageFile", "Seçilmiş  şəkilin həcmi 100KB-dan çox ola bilməz");
                        return View("Profile", memberProfileVM);
                    }
                    if (appUser.UserImage != null)
                    {
                        Helper.Helper.DeleteFile(_env, appUser.UserImage, "assets", "images");
                    }
                    appUser.UserImage = member.UserImageFile.CreateFile(_env, "assets", "images");
                }
            }

            appUser.FullName = member.FullName;
            appUser.UserName = member.UserName;
            appUser.Email = member.Email;
            appUser.Adress = member.Adress;
            appUser.City = member.City;
            appUser.PhoneNumber = member.PhoneNumber; 

            IdentityResult identityResult=await _userManager.UpdateAsync(appUser);

            if (!identityResult.Succeeded)
            {
                foreach (var item in identityResult.Errors)
                {
                    ModelState.AddModelError("",item.Description);
                }
                return View("Profile", memberProfileVM);
            }
            if (member.Password!=null)
            {
                if (string.IsNullOrWhiteSpace(member.CurrentPassword))
                {
                    ModelState.AddModelError("CurrentPassword", "Hazırki şifrənizi daxil edin");
                    return View("Profile", memberProfileVM);
                }

                if (!await _userManager.CheckPasswordAsync(appUser,member.CurrentPassword))
                {
                    ModelState.AddModelError("CurrentPassword", "Şifrəniz yanlışdır");
                    return View("Profile",memberProfileVM);
                }
                string token = await _userManager.GeneratePasswordResetTokenAsync(appUser);
                identityResult = await _userManager.ResetPasswordAsync(appUser,token,member.Password);

                if (!identityResult.Succeeded)
                {
                    foreach (var item in identityResult.Errors)
                    {
                        ModelState.AddModelError("",item.Description);
                    }
                    return View("Profile");
                }

                await _signInManager.PasswordSignInAsync(appUser, member.Password,true,true);
            }

            //await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(appUser,true);

            return RedirectToAction("Profile");
        }



        public IActionResult Index()
        {
            return View();
        }



        #region Create Role

        //public async Task<IActionResult> CreateRole()
        //{
        //    await _roleManager.CreateAsync(new IdentityRole { Name = "SuperAdmin" });
        //    await _roleManager.CreateAsync(new IdentityRole { Name = "Market" });
        //    await _roleManager.CreateAsync(new IdentityRole { Name = "Member" });

        //    return Ok();

        //}
        #endregion
    }
}
