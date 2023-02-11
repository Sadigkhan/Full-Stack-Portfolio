using Kontakt.DAL;
using Kontakt.Extensions;
using Kontakt.Helpers;
using Kontakt.Models;
using Kontakt.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using static Kontakt.Helpers.Helper;
using static System.Net.Mime.MediaTypeNames;

namespace Kontakt.Controllers
{
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

        //[Authorize(Roles = "Member")]
        public IActionResult MobilMenu()
        {
            List<Category> categories = _context.Categories.Where(x => x.IsMain&& !x.IsDeleted).ToList();
            AppUser appUser = null;
            if (User.Identity.IsAuthenticated)
            {
                appUser = _userManager.Users.FirstOrDefault(u => u.UserName == User.Identity.Name && !u.isAdmin);
            }
            if (appUser!=null)
            {
                ViewBag.AppUser = "true";
            }
            else
            {
                ViewBag.AppUser = "false";
            }
            return PartialView("_MobilMenuPartial",categories);
        }
        public async Task<IActionResult>  AccountPartial()
        {
            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name && !u.isAdmin);
            ViewBag.OrderCount = _context.Orders.Where(o=>o.AppUserId==appUser.Id).Count();
            ViewBag.BasketCount = _context.Baskets.Where(o => o.AppUserId == appUser.Id).Count();
            ViewBag.RevCount =  _context.Reviews.Where(x => x.AppUserId == appUser.Id).Count();
            ViewBag.LikeCount = _context.Likes.Where(x => x.AppUserId == appUser.Id).Count();
            ViewBag.DisLikeCount = _context.DisLikes.Where(x => x.AppUserId == appUser.Id).Count();
            string cookieWish = HttpContext.Request.Cookies["Wish"];

            List<WishVM> wishVMs = null;

            if (cookieWish != null)
            {
                wishVMs = JsonConvert.DeserializeObject<List<WishVM>>(cookieWish);
            }
            else
            {
                wishVMs = new List<WishVM>();
            }
            ViewBag.WishCount = wishVMs.Count();
            return PartialView("_AccountRightPartial");
        }
        public async Task<IActionResult>  EditAccountInfo()
        {
            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name && !u.isAdmin);
            if (appUser == null) return RedirectToAction("index", "home");
            UserProfileVM userProfileVM = new UserProfileVM
            {
                Member = new UserUpdateVM
                {

                    Name = appUser.Name,
                    SurName = appUser.SurName,
                    UserName = appUser.UserName,
                    Email = appUser.Email,
                    Address = appUser.Address,
                    City = appUser.City,
                    Country = appUser.Country,
                    ZipCode = appUser.ZipCode,
                    PhoneNumber = appUser.PhoneNumber,
                    State = appUser.State,
                    Image = appUser.Image,
                    ParentName = appUser.ParentName,
                    Gender = appUser.Gender,
                    Birthday = appUser.Birthday



                },
                

            };

            return PartialView("_EditAccountInfo",userProfileVM);
        }
        public async Task<IActionResult> EditAccountAdress()
        {
            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name && !u.isAdmin);
            if (appUser == null) return RedirectToAction("index", "home");
            UserAdressUpdate UserAdressUpdate = new UserAdressUpdate
            {
                

                    
                    Address = appUser.Address,
                    City = appUser.City,
                    Country = appUser.Country,
                    ZipCode = appUser.ZipCode,
                    State=appUser.State,
                  

            };

            return PartialView("_EditAccountAdress", UserAdressUpdate);
        }
        public async Task<IActionResult> EditAccountAuth()
        {
            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name && !u.isAdmin);
            if (appUser == null) return RedirectToAction("index", "home");
            UserAuthUpdateVM userAuthUpdate = new UserAuthUpdateVM
            {



                Email=appUser.Email,
                
                

            };

            return PartialView("_EditAccountAuth", userAuthUpdate);
        }
        public async Task<IActionResult> EditAccountImg()
        {
            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name && !u.isAdmin);
            if (appUser == null) return RedirectToAction("index", "home");
            UserImgUpdateVM userImgUpdate = new UserImgUpdateVM
            {



                Image = appUser.Image,



            };

            return PartialView("_EditAccountImage", userImgUpdate);
        }
        public async Task<IActionResult> ForgotPassword()
        {
            

            return PartialView("_ForgotPasswordPartial");
        }

        public async Task<IActionResult> MyAccount(string from,string to)
        {
            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name && !u.isAdmin);
            
            if (appUser == null) return RedirectToAction("index", "home");
            ViewBag.to = to;

            string cookieWish = HttpContext.Request.Cookies["Wish"];

            List<WishVM> wishVMs = null;

            if (cookieWish != null)
            {
                wishVMs = JsonConvert.DeserializeObject<List<WishVM>>(cookieWish);
            }
            else
            {
                wishVMs = new List<WishVM>();
            }

            foreach (WishVM wishVM in wishVMs)
            {
                Product dbProduct = await _context.Products
                      .Include(x => x.ProductDetails).ThenInclude(x => x.DetailKey).ThenInclude(x => x.DetailValues)
                    .Include(x => x.Reviews)
                    .FirstOrDefaultAsync(p => p.Id == wishVM.ProductId);
                wishVM.Image = dbProduct.MainImage;
                wishVM.Price = (double)(dbProduct.DiscountPrice > 0 ? dbProduct.DiscountPrice : dbProduct.Price);
                wishVM.Title = dbProduct.Title;
                wishVM.Reviews = await _context.Reviews.Where(r => r.ProductId == dbProduct.Id).ToListAsync();
                wishVM.Product = dbProduct;
            }

            List<LikeVM> likeVMs = new List<LikeVM>();

            List<Like> likes = await _context.Likes.Where(x => x.AppUserId == appUser.Id).ToListAsync();

            List<DisLikeVM> disLikeVMs = new List<DisLikeVM>();

            List<DisLike> disLikes = await _context.DisLikes.Where(x => x.AppUserId == appUser.Id).ToListAsync();

            foreach (Like like in likes)
            {
                Product dbProduct = await _context.Products
                     .Include(x => x.ProductDetails).ThenInclude(x => x.DetailKey).ThenInclude(x => x.DetailValues)
                   .Include(x => x.Reviews)
                   .FirstOrDefaultAsync(p => p.Id == like.ProductId);
                var Image = dbProduct.MainImage;
                var Price = (double)(dbProduct.DiscountPrice > 0 ? dbProduct.DiscountPrice : dbProduct.Price);
                var Title = dbProduct.Title;
                var Reviews = await _context.Reviews.Where(r => r.ProductId == dbProduct.Id).ToListAsync();
                var Product = dbProduct;


                LikeVM likeVM = new LikeVM()
                {
                    Image=Image,
                    Price=Price,
                    Title=Title,
                    Reviews=Reviews,
                    Product=Product
                };

                likeVMs.Add(likeVM);
                
            }

            foreach (DisLike disLike in disLikes)
            {
                Product dbProduct = await _context.Products
                     .Include(x => x.ProductDetails).ThenInclude(x => x.DetailKey).ThenInclude(x => x.DetailValues)
                   .Include(x => x.Reviews)
                   .FirstOrDefaultAsync(p => p.Id == disLike.ProductId);
                var Image = dbProduct.MainImage;
                var Price = (double)(dbProduct.DiscountPrice > 0 ? dbProduct.DiscountPrice : dbProduct.Price);
                var Title = dbProduct.Title;
                var Reviews = await _context.Reviews.Where(r => r.ProductId == dbProduct.Id).ToListAsync();
                var Product = dbProduct;


                DisLikeVM disLikeVM = new DisLikeVM()
                {
                    Image = Image,
                    Price = Price,
                    Title = Title,
                    Reviews = Reviews,
                    Product = Product
                };

                disLikeVMs.Add(disLikeVM);

            }

            string cookieBasket = HttpContext.Request.Cookies["basket"];

            List<BasketVM> basketVMs = null;

            if (cookieBasket != null)
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(cookieBasket);
            }
            else
            {
                basketVMs = new List<BasketVM>();
            }

            foreach (BasketVM basketVM in basketVMs)
            {
                Product dbProduct = await _context.Products
                    .Include(x => x.ProductDetails).ThenInclude(x => x.DetailKey).ThenInclude(x => x.DetailValues).OrderBy(x => x.UpdatedAt)
                    .Include(x => x.Reviews)
                    .FirstOrDefaultAsync(p => p.Id == basketVM.ProductId);
                basketVM.Image = dbProduct.MainImage;
                basketVM.Price = (double)(dbProduct.DiscountPrice > 0 ? dbProduct.DiscountPrice : dbProduct.Price);
                basketVM.Title = dbProduct.Title;
                basketVM.Reviews = await _context.Reviews.Where(r => r.ProductId == dbProduct.Id).ToListAsync();
                basketVM.Product = dbProduct;
            }


            UserProfileVM userProfileVM = new UserProfileVM
            {
                Member = new UserUpdateVM
                {

                    Name = appUser.Name,
                    SurName = appUser.SurName,
                    UserName = appUser.UserName,
                    Email = appUser.Email,
                    Address = appUser.Address,
                    City = appUser.City,
                    Country = appUser.Country,
                    ZipCode = appUser.ZipCode,
                    PhoneNumber = appUser.PhoneNumber,
                    State = appUser.State,
                    Image = appUser.Image,
                    ParentName = appUser.ParentName,
                    Gender = appUser.Gender,
                    Birthday = appUser.Birthday



                },
                Orders = await _context.Orders
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Product)
                .Include(o => o.AppUser)
                .Where(o => !o.IsDeleted && o.AppUserId == appUser.Id).OrderByDescending(x => x.CreatedAt).ToListAsync(),
                WishVMs = wishVMs,
                BasketVMs = basketVMs,
                Reviews = await _context.Reviews
                .Include(x=>x.Product).ThenInclude(x=>x.ProductDetails).ThenInclude(x=>x.DetailKey).ThenInclude(x=>x.DetailValues)
                .Where(x => x.AppUserId == appUser.Id).ToListAsync(),
                LikeVMs=likeVMs,
                DisLikeVMs=disLikeVMs

                



            };
            if (from=="account")
            {

                return PartialView("_MyAccountPartial",userProfileVM);
            }
            else if (from == "order")
            {

                return PartialView("_MyOrdersPartial", userProfileVM);
            }
            else if (from == "favorite")
            {

                return PartialView("_MyFavoritePartial", userProfileVM.WishVMs);
            }
            else if (from == "compare")
            {

                return PartialView("_MyComparePartial", userProfileVM.BasketVMs);
            }
            else if (from == "view")
            {

                return PartialView("_MyViewPartial", userProfileVM.LikeVMs);
            }
            else if (from == "comment")
            {

                return PartialView("_MyCommentPartial", userProfileVM.Reviews);
            }
            else
            if (from == "dislike")
            {

                return PartialView("_MyDislikePartial", userProfileVM.DisLikeVMs);
            }




            return View(userProfileVM);
        }
        [HttpPost]
        public async Task<IActionResult> AccountInfoUpdate([FromBody] UserInfoUpdateVM member)
        {
            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name && !u.isAdmin);

            if (appUser == null) return RedirectToAction("index", "home");
            //UserProfileVM userProfileVM = new UserProfileVM
            //{
            //    MemberInfo = member
            //};
            if (member==null)
            {
                return Json(new { status = 400, message = "Doğum tarixi düzgün daxil edilmeyib  " });
            }
            if (string.IsNullOrWhiteSpace(member.Name))
            {
                return Json(new { status = 400, message = "Ad mütləqdir" });
            }
            if (string.IsNullOrWhiteSpace(member.SurName))
            {
                return Json(new { status = 400, message = "Soyad mütləqdir" });
            }

            if (string.IsNullOrWhiteSpace(member.ParentName))
            {
                member.ParentName = null;
            }
            if (string.IsNullOrWhiteSpace(member.PhoneNumber))
            {
                member.PhoneNumber = null;
            }
            
            appUser.ParentName = member.ParentName;
            appUser.Gender = member.Gender;
            appUser.Birthday = member.Birthday;
            appUser.Name = member.Name;
            appUser.SurName = member.SurName;
            
            appUser.PhoneNumber = member.PhoneNumber;

            IdentityResult identityResult = await _userManager.UpdateAsync(appUser);

            if (!identityResult.Succeeded)
            {
                foreach (var item in identityResult.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return Json(new { status = 400, message = $"İcazə verilməyən məlumatlar daxil etmisiniz" });
            }



            return Json(new { status = 200, message = $"{member.Name} {member.SurName}" });
        }
        [HttpPost]
        public async Task<IActionResult> AccountAdressUpdate([FromBody] UserAdressUpdate member)
        {
            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name && !u.isAdmin);
            if (!ModelState.IsValid) return Json(new { status = 400, message = "İcazə verilməyən məlumatlar daxil etmisiniz !!!" });
            if (appUser == null) return RedirectToAction("index", "home");


            if (string.IsNullOrWhiteSpace(member.Address))
            {
                member.Address =null;
            }
            if (string.IsNullOrWhiteSpace(member.City))
            {
                member.City = null;
            }
            if (string.IsNullOrWhiteSpace(member.ZipCode))
            {
                member.ZipCode = null;
            }
            if (string.IsNullOrWhiteSpace(member.Country))
            {
                member.Country = null;
            }
            if (string.IsNullOrWhiteSpace(member.State))
            {
                member.State = null;
            }

            UserAdressUpdate userAdressUpdate = new UserAdressUpdate
            {
                Address = member.Address,
                City = member.City,
                ZipCode = member.ZipCode,
                Country = member.Country,
                State = member.State

            };
            


            
           

            appUser.Address = userAdressUpdate.Address;
            appUser.City = userAdressUpdate.City;
            appUser.ZipCode = userAdressUpdate.ZipCode;
            appUser.Country = userAdressUpdate.Country;
            appUser.State = userAdressUpdate.State;
            

            IdentityResult identityResult = await _userManager.UpdateAsync(appUser);


            return Json(new { status = 200, message = "" });
        }

        [HttpPost]
        public async Task<IActionResult> AccountAuthUpdate([FromBody] UserAuthUpdateVM member)
        {
            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name && !u.isAdmin);
            if (!ModelState.IsValid) return Json(new { status = 400, message = "İcazə verilməyən məlumatlar daxil etmisiniz !!!" });
            if (appUser == null) return RedirectToAction("index", "home");
            

            if (!(string.IsNullOrWhiteSpace(member.CurrentPassword)))
            {
                //IdentityResult identityResult = await _userManager.UpdateAsync(appUser);

                

                if (!await _userManager.CheckPasswordAsync(appUser, member.CurrentPassword))
                {
                    return Json(new { status = 400, message = $"Current Password Yanlisdir" });
                }

                else
                {
                    if (string.IsNullOrWhiteSpace(member.Email))
                    {
                        return Json(new { status = 400, message = "E-poçt mütləqdir" });
                    }

                    if (appUser.NormalizedEmail != member.Email.ToUpperInvariant() && await _userManager.Users.AnyAsync(u => u.NormalizedEmail == member.Email.ToUpperInvariant()))
                    {
                        return Json(new { status = 400, message = $"{member.Email} - E-poçt artıq mövcuddur" });
                    }

                    appUser.Email = member.Email;
                    IdentityResult identityResult = await _userManager.UpdateAsync(appUser);
                    if (!(string.IsNullOrWhiteSpace(member.Password)))
                    {
                        string token = await _userManager.GeneratePasswordResetTokenAsync(appUser);
                         identityResult = await _userManager.ResetPasswordAsync(appUser, token, member.Password);

                        if (!identityResult.Succeeded)
                        {
                            return Json(new { status = 400, message = "opps" });
                        }

                        if (!await _userManager.CheckPasswordAsync(appUser, member.ConfirmPassword))
                        {
                            return Json(new { status = 400, message = "Şifrə düzgün deyil" });
                        }
                    }
                }

                
                
                

                

                
                
            }
            else
            {
                return Json(new { status = 400, message = "Şifrə mütləqdir" });
            }

            


            return Json(new { status = 200, message = "" });
        }

        

        public IActionResult LoginReg()
        {
            return PartialView("_LoginRegPartial");
        }
        
        public IActionResult Login()
        {
            return PartialView("_LoginPartial");
        }
        
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginVM login)
        {


            

            if (!ModelState.IsValid) return Json(new { status = 400, message = "Email or Password is wrong" });
            AppUser user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Email or Password is wrong");

                return Json(new { status = 400, message = "Email or Password is wrong" });
            }
            if (user.IsDeleted)
            {
                ModelState.AddModelError("", "User Deactived");
                return Json(new { status = 400, message = "User Deactived" });
            }
            if ((await _userManager.GetRolesAsync(user))[0] != Roles.Member.ToString())
            {
                return Json(new { status = 400, message = "Sizə bura daxil olmaq üçün icazə yoxdur" });
            }
            var signInResult = await _signInManager.PasswordSignInAsync(user, login.Password, login.RememberMe, true);

            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError("", $"User is blocket wait 5 min");
                return Json(new { status = 400, message = "User is blocket wait 5 min" });
            }

            if (!signInResult.Succeeded)
            {
               
                return Json(new { status = 400, message = "Email or Password is wrong" });

            }
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return Json(new { status = 400, message = "Zəhmət olmasa ilk öncə hesabınızı təsdiqləyin" });
            }
            if ((await _userManager.GetRolesAsync(user))[0] == Roles.Member.ToString())
            {
                string coockieBasket = HttpContext.Request.Cookies["basket"];
              

                if (!string.IsNullOrWhiteSpace(coockieBasket))
                {
                    List<BasketVM> basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(coockieBasket);
                    List<Basket> baskets = new List<Basket>();
                    List<Basket> existedBasket = await _context.Baskets.Where(b => b.AppUserId == user.Id).ToListAsync();



                    if (basketVMs.Count>0)
                    {
                        _context.Baskets.RemoveRange(existedBasket);
                        foreach (BasketVM basketVM in basketVMs)
                        {

                            Basket basket = new Basket
                            {

                                AppUserId = user.Id,
                                ProductId = basketVM.ProductId,
                                Count = basketVM.Count,
                                CreatedAt = DateTime.UtcNow.AddHours(4)
                            };

                            baskets.Add(basket);



                        }

                        if (baskets.Count > 0)
                        {
                            await _context.Baskets.AddRangeAsync(baskets);
                            await _context.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        List<Basket> existedBasket1 = await _context.Baskets.Where(b => b.AppUserId == user.Id).ToListAsync();

                        List<BasketVM> basketVMss = new List<BasketVM>();

                        if (existedBasket.Count > 0)
                        {
                            foreach (Basket basket in existedBasket)
                            {

                                BasketVM basketVM = new BasketVM
                                {
                                    ProductId = (int)basket.ProductId,
                                    Count = (int)basket.Count,

                                };

                                basketVMss.Add(basketVM);

                            }

                            coockieBasket = JsonConvert.SerializeObject(basketVMss);
                            HttpContext.Response.Cookies.Append("basket", coockieBasket);
                        }
                    }
                     
                    
                }
                else
                {
                    
                    List<Basket> existedBasket = await _context.Baskets.Where(b => b.AppUserId == user.Id).ToListAsync();

                    List<BasketVM> basketVMs = new List<BasketVM>();

                    if (existedBasket.Count>0)
                    {
                        foreach (Basket basket in existedBasket)
                        {

                            BasketVM basketVM = new BasketVM
                            {
                                ProductId = (int)basket.ProductId,
                                Count = (int)basket.Count,

                            };

                            basketVMs.Add(basketVM);

                        }

                        coockieBasket = JsonConvert.SerializeObject(basketVMs);
                        HttpContext.Response.Cookies.Append("basket", coockieBasket);
                    }
                    
                }
            }

            
            
                return Json(new { status = 200, message = "/home/index" });
            


        }


        public IActionResult Register()
        {
            return PartialView("_RegisterPartial");
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromBody] RegisterVM register)
        {
            //if (!ModelState.IsValid) return Json(new { status = 400, message = $"Məlumatlar düzgün daxil edilməyib" }); 

            AppUser newUser = new AppUser
            {
                Name = register.Name,
                SurName = register.SurName,
                UserName = register.UserName,
                Email = register.Email

            };
            if (!register.Email.Contains("@"))
            {
                return Json(new { status = 400, message = $"{register.Email} - E-poçt düzgün formatda deyil" });
            }
            if (await _userManager.Users.AnyAsync(u => u.NormalizedEmail == register.Email.ToUpperInvariant()))
            {
                return Json(new { status = 400, message = $"{register.Email} - E-poçt artıq mövcuddur" });
            }

            if (await _userManager.Users.AnyAsync(u => u.NormalizedUserName == register.UserName.ToUpperInvariant()))
            {
                return Json(new { status = 400, message = $"{register.UserName} - İstifadəçi adı artıq mövcuddur" });
            }
            if (register.Password.ToString()!=register.CheckPassword.ToString())
            {
                return Json(new { status = 400, message = $"Təkrar şifrə eyni deyil" });
            }
            IdentityResult identityResult = await _userManager.CreateAsync(newUser, register.Password);
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    return Json(new { status = 400, message = $"{error.Description}" });
                }
                

            }


            string token = Guid.NewGuid().ToString();
            newUser.EmailConfirmationToken = token;
            await _userManager.AddToRoleAsync(newUser, Roles.Member.ToString());

            var link = Url.Action(nameof(VerifyEmail), "Account", new { id = newUser.Id, token }, Request.Scheme, Request.Host.ToString());

            EmailVM email = _config.GetSection("Email").Get<EmailVM>();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(email.SenderEmail, email.SenderName);
            mail.To.Add(newUser.Email);
            mail.Subject = "VerifyEmail";
            string body = "";
            using (StreamReader reader = new StreamReader("wwwroot/user/assets/Template/verifyemail.html"))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{link}", link);
            body = body.Replace("{name}", newUser.Name);
            body = body.Replace("{surname}", newUser.SurName);
            body = body.Replace("{email}", newUser.Email);


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

            return Json(new { status = 200, message = $"Qeydiyyat uğurla tamamlandı {newUser.Email} E-poçt ünvanına daxil olaraq hesabı təsdiq edin " });
        }


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
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("index","home");
                
            }



            return BadRequest();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> UserDeleteImage()
        {
            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name && !u.isAdmin);
            
            if (appUser == null) return RedirectToAction("index", "home");

            

            if (appUser.Image!=null)
            {
                Helper.DeleteFile(_env, appUser.Image, "user", "assets", "img", "userimg");
                appUser.Image = null;
                IdentityResult identityResult = await _userManager.UpdateAsync(appUser);

                return Json(new { status = 200, message = $"/User/assets/img/userimg/icone-utilisateur-rouge.png" });
            }
            else
            {
                return Json(new { status = 400, message = $"" });
            }
            


            
        }

        public IActionResult ResetPassword() => View();

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordVM reset)
        {
            if (string.IsNullOrWhiteSpace(reset.Email))
            {
                return Json(new { status = 400, message = $"E-poçt ünvanını daxil edin" });
            }
            AppUser user = await _userManager.FindByEmailAsync(reset.Email);
            if (user == null)
            {
                
                return Json(new { status = 400, message = $"{reset.Email} - uyğun hesab tapılmadı" });
            }
            string token = Guid.NewGuid().ToString();
            user.PasswordResetToken = token;
            await _userManager.UpdateAsync(user);
            var link = Url.Action(nameof(Index), "home", new { id = user.Id, token = user.PasswordResetToken }, Request.Scheme, Request.Host.ToString());
            
            EmailVM email = _config.GetSection("Email").Get<EmailVM>();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(email.SenderEmail, email.SenderName);
            mail.To.Add(user.Email);
            mail.Subject = "Reset Password";
            string body = "";
            using (StreamReader reader = new StreamReader("wwwroot/user/assets/Template/resetpass.html"))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{link}", link);
            body = body.Replace("{name}", user.Name);
            body = body.Replace("{surname}", user.SurName);


            //mail.Body = $"<a href=\"{link}\">Verify</a>";
            mail.Body = body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = email.Server;
            smtp.Port = email.Port;
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential(email.SenderEmail, email.Password);
            smtp.Send(mail);

            return Json(new { status = 200, message = $"Şifrənin yenilənmə linki {user.Email} E-poçt ünvanına göndərildi E-poçt ünvanına  daxil olaraq şifrəni bərpa edin " });
        }

        public IActionResult NewPassword(ResetPasswordVM resetPasswordVM)
        {


            return PartialView("_PasswordResetPartial",resetPasswordVM);
        }

        [HttpPost]
        
        public async Task<IActionResult> NewPasswordPost([FromBody] ResetPasswordVM reset)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { status = 400, message = $"Təkrar şifrə eyni deyil" });
            }
            if (reset.Id == null)
            {
                return Json(new { status = 400, message = "Hesab id -si düzgün deyil" });
            }
            AppUser user = await _userManager.FindByIdAsync(reset.Id);
            if (user == null)
            {
                return Json(new { status = 400, message = "Hesab id tapılmadı" });
            }
            if (user.PasswordResetToken != reset.Token)
            {
                return Json(new { status = 400, message = "Bu link istifadə olunub" });
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            IdentityResult result = await _userManager.ResetPasswordAsync(user, resetToken, reset.Password);



            if (result.Succeeded)
            {
                string passwordResetToken = Guid.NewGuid().ToString();
                user.PasswordResetToken = passwordResetToken;
                await _userManager.UpdateAsync(user);
                return Json(new { status = 200, message = "Şifrəniz uğurla dəyişdirildi" });
            }
            return Json(new { status = 400, message = "Şifrə parametrləri düzgün deyil" });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AccountImgUpdate(UserImgUpdateVM member)
        {
            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name && !u.isAdmin);

            if (appUser == null) return RedirectToAction("index", "home");

            

            if (member.ImageFile != null)
            {
                if (!member.ImageFile.CheckFileContentType("image/jpeg"))
                {
                   
                    return Json(new { status = 400, message = $"Secilen Seklin Novu Uygun" });
                }

                if (!member.ImageFile.CheckFileSize(5000))
                {
                   
                    return Json(new { status = 400, message = $"Secilen Seklin Olcusu Maksimum 500 Kb Ola Biler" });
                }
                if (appUser.Image != null)
                {
                    Helper.DeleteFile(_env, appUser.Image, "user", "assets", "img", "userimg");
                }


                appUser.Image = member.ImageFile.CreateFile(_env, "user", "assets", "img", "userimg");
            }


            IdentityResult identityResult = await _userManager.UpdateAsync(appUser);

            if (!identityResult.Succeeded)
            {
                foreach (var item in identityResult.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return Json(new { status = 400, message = $"İcazə verilməyən məlumatlar daxil etmisiniz" });
            }



            return RedirectToAction( "myaccount", "account");
        }

        #region Create Role
        //public async Task CreateRoles()
        //{
        //    foreach (var role in Enum.GetValues(typeof(Roles)))
        //    {
        //        if (!(await _roleManager.RoleExistsAsync(role.ToString())))
        //        {
        //            await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
        //        }
        //    }
        //}
        #endregion


        
    }
}
