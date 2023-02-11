using Kontakt.DAL;
using Kontakt.Models;
using Kontakt.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontakt.Controllers
{
    public class WishController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public WishController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> GetWishCount()
        {
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

            return Json(new { status = 200, message = $"{wishVMs.Count}" });
        }

        public async Task<IActionResult> GetMiniWish()
        {
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



            return PartialView("_GetMiniWish", wishVMs);
        }

        public async Task<IActionResult> DeleteWish(int? id)
        {
            if (id == null) return BadRequest();

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            string cookieWish = HttpContext.Request.Cookies["Wish"];

            List<WishVM> wishVMs = null;

            if (cookieWish != null)
            {
                wishVMs = JsonConvert.DeserializeObject<List<WishVM>>(cookieWish);

                WishVM wishVM = wishVMs.FirstOrDefault(b => b.ProductId == id);

                if (wishVM == null)
                {
                    return NotFound();
                }

                wishVMs.Remove(wishVM);
            }
            else
            {
                return BadRequest();
            }

            cookieWish = JsonConvert.SerializeObject(wishVMs);
            HttpContext.Response.Cookies.Append("Wish", cookieWish);

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
            //AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name && !u.isAdmin);

            //if (appUser != null)
            //{
            //    List<Basket> existedBasket = await _context.Baskets.Where(b => b.AppUserId == appUser.Id).ToListAsync();

            //    if (existedBasket.Any(b => b.ProductId == id))
            //    {
            //        foreach (Basket basket in existedBasket)
            //        {
            //            if (basket.ProductId == id)
            //            {
            //                _context.Baskets.Remove(basket);
            //            }

            //        }
            //        await _context.SaveChangesAsync();
            //    }
            //}


            return PartialView("_GetMiniWish", wishVMs);
        }
    }
}
