using JuanBackFinal.DAL;
using JuanBackFinal.Models;
using JuanBackFinal.ViewModels.Basket;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JuanBackFinal.Controllers
{
    public class BasketController : Controller
    {

        private readonly JuanAppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public BasketController(JuanAppDbContext context,UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult>  Index()
        {
            List<BasketVM> basketVMs = null;
            if (!User.Identity.IsAuthenticated)
            {
            string cookieBasket = HttpContext.Request.Cookies["basket"];
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
                Product dbProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == basketVM.ProductId);
                basketVM.Image = dbProduct.MainImage;
                basketVM.Price = dbProduct.DiscountPrice > 0 ? dbProduct.DiscountPrice : dbProduct.SalePrice;
                basketVM.Name = dbProduct.Name;
            }

            }

            else
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                List<Basket> baskets1 = _context.Baskets.Include(b => b.Product).Where(b => b.AppUserId == user.Id).ToList();
                List<BasketVM> basketss = new List<BasketVM>();
                foreach (var item in baskets1)
                {
                    BasketVM basketVM = new BasketVM
                    {
                        Name = item.Product.Name,
                        Price = item.Product.SalePrice,
                        Count = item.Count,
                        Image = item.Product.MainImage,
                        ProductId=item.Product.Id

                    };
                    basketss.Add(basketVM);
                }
                return View( basketss);
            }

            return View(basketVMs);
        }
      



        public async Task<IActionResult> Update(int? id, int? count)
        {
            List<BasketVM> basketVMs = null;
            
            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            if (!User.Identity.IsAuthenticated)
            {
            string cookieBasket = HttpContext.Request.Cookies["basket"];

            

            if (cookieBasket != null)
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(cookieBasket);

                if (!basketVMs.Any(b => b.ProductId == id))
                {
                    return NotFound();
                }

                basketVMs.Find(b => b.ProductId == id).Count = (int)count;
            }
            else
            {
                return BadRequest();
            }

            cookieBasket = JsonConvert.SerializeObject(basketVMs);
            HttpContext.Response.Cookies.Append("basket", cookieBasket);

            foreach (BasketVM basketVM in basketVMs)
            {
                Product dbProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == basketVM.ProductId);
                basketVM.Image = dbProduct.MainImage;
                basketVM.Price = dbProduct.DiscountPrice > 0 ? dbProduct.DiscountPrice : dbProduct.SalePrice;
                basketVM.Name = dbProduct.Name;
               
            }

            }
            else
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                Basket basket = _context.Baskets.FirstOrDefault(b => b.ProductId == id && b.AppUserId == user.Id);
                if (basket == null) return NotFound();
                basket.Count = (int)count;
                _context.SaveChanges();
                List<Basket> baskets = _context.Baskets.Include(b=>b.Product).Where(b=>b.AppUserId==user.Id).ToList();

                List<BasketVM> basketVMs1 = new List<BasketVM>();
                foreach (Basket baskett in baskets)
                {
                    BasketVM basketVM = new BasketVM
                    {
                        Count=baskett.Count,
                        Name=baskett.Product.Name,
                        Image=baskett.Product.MainImage,
                        Price=baskett.Product.SalePrice,
                        ProductId=baskett.Product.Id
                    };
                    basketVMs1.Add(basketVM);
                }
                return PartialView("_BasketIndexPartial", basketVMs1);
            }

            return PartialView("_BasketIndexPartial", basketVMs);
        }


        public async Task<IActionResult> Delete(int id)
        {
      

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            string cookieBasket = HttpContext.Request.Cookies["basket"];

            List<BasketVM> basketVMs = null;

            if (cookieBasket != null)
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(cookieBasket);

                if (!basketVMs.Any(b => b.ProductId == id))
                {
                    return NotFound();
                }
                BasketVM RemoveItem = basketVMs.Find(b => b.ProductId == id);
                basketVMs.Remove(RemoveItem);
               
            }
            else
            {
                return BadRequest();
            }

            cookieBasket = JsonConvert.SerializeObject(basketVMs);
            HttpContext.Response.Cookies.Append("basket", cookieBasket);

            foreach (BasketVM basketVM in basketVMs)
            {
                Product dbProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == basketVM.ProductId);
                basketVM.Image = dbProduct.MainImage;
                basketVM.Price = dbProduct.DiscountPrice > 0 ? dbProduct.DiscountPrice : dbProduct.SalePrice;
                basketVM.Name = dbProduct.Name;

            }

            return PartialView("_BasketIndexPartial", basketVMs);
        }

        public async Task<IActionResult> DeleteItem(int id)
        {


            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            string cookieBasket = HttpContext.Request.Cookies["basket"];

            List<BasketVM> basketVMs = null;

            if (cookieBasket != null)
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(cookieBasket);

                if (!basketVMs.Any(b => b.ProductId == id))
                {
                    return NotFound();
                }
                BasketVM RemoveItem = basketVMs.Find(b => b.ProductId == id);
                basketVMs.Remove(RemoveItem);
            }
            else
            {
                return BadRequest();
            }

            cookieBasket = JsonConvert.SerializeObject(basketVMs);
            HttpContext.Response.Cookies.Append("basket", cookieBasket);

            foreach (BasketVM basketVM in basketVMs)
            {
                Product dbProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == basketVM.ProductId);
                basketVM.Image = dbProduct.MainImage;
                basketVM.Price = dbProduct.DiscountPrice > 0 ? dbProduct.DiscountPrice : dbProduct.SalePrice;
                basketVM.Name = dbProduct.Name; 

            }

            return PartialView("_BasketPartial", basketVMs);
        }
    }
}
