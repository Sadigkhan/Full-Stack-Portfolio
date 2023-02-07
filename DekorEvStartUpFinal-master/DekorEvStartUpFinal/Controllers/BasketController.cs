using DekorEvStartUpFinal.DAL;
using DekorEvStartUpFinal.Models;
using DekorEvStartUpFinal.ViewModels.Basket;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DekorEvStartUpFinal.Controllers
{
    public class BasketController : Controller
    {
        private readonly DekorEvStartupAppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public BasketController(DekorEvStartupAppDbContext context, UserManager<AppUser> userManager)
        {

            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {

            List<BasketVM> basketVMs = new List<BasketVM>();

            string cookieBasket = HttpContext.Request.Cookies["basket"];

            if (!string.IsNullOrWhiteSpace(cookieBasket))
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(cookieBasket);
            }
            else
            {
                basketVMs = new List<BasketVM>();
            }
            foreach (BasketVM compareVM in basketVMs)
            {
                Product dbProduct = await _context.Products
                    .Include(p => p.ProductColorMaterials)
                    .Include(p => p.ViewCount).FirstOrDefaultAsync(p => p.Id == compareVM.ProductId);

                compareVM.Price = dbProduct.DiscountPrice > 0 ? dbProduct.DiscountPrice : dbProduct.SalePrice;
                compareVM.Name = dbProduct.Name;
                compareVM.Image = dbProduct.ProductColorMaterials.FirstOrDefault(p => p.MaterialId == compareVM.MaterialId && p.ColorId == compareVM.ColorId)?.Image;



               
            }

            return View(basketVMs);
        }

        public async Task<IActionResult> Update(int? id, int colorId, int materialId, int count=1)
        {
            AppUser user = await _userManager.GetUserAsync(User);
            if (id == null) return BadRequest();

            if (count <= 0)
            {
                count = 1;
            }
            
            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            string cookieBasket = HttpContext.Request.Cookies["basket"];

            List<BasketVM> basketVMs = null;

            if (user != null)
            {
                Basket basketItem = await _context.Baskets.FirstOrDefaultAsync(b => b.AppUserId == user.Id && b.ProductId == product.Id && b.ColorId == colorId && b.MaterialId == materialId);

                if (basketItem == null)
                {
                    return NotFound();
                }

                basketItem.Count = (int)count;
                await _context.SaveChangesAsync();
            }

            if (!string.IsNullOrWhiteSpace(cookieBasket))
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(cookieBasket);

                BasketVM basketVM = basketVMs.FirstOrDefault(b => b.ProductId == id && b.ColorId == colorId && b.MaterialId == materialId);

                if (basketVM == null)
                {
                    return NotFound();
                }

                basketVM.Count = count;
            }
            else
            {
                return BadRequest();
            }

            cookieBasket = JsonConvert.SerializeObject(basketVMs);
            HttpContext.Response.Cookies.Append("basket", cookieBasket);

            foreach (BasketVM basketVM in basketVMs)
            {
                Product dbProduct = await _context.Products.Include(p=>p.ProductColorMaterials).FirstOrDefaultAsync(p => p.Id == basketVM.ProductId);
                basketVM.Image = dbProduct.ProductColorMaterials?.FirstOrDefault(p => p.MaterialId == basketVM.MaterialId && p.ColorId == basketVM.ColorId)?.Image;
                basketVM.Price = (double)(dbProduct.DiscountPrice > 0 ? dbProduct.DiscountPrice : dbProduct.SalePrice);
                basketVM.Name = dbProduct.Name;
            }

            return PartialView("_BasketIndexPartial", basketVMs);
        }

        public async Task<IActionResult> DeleteCard(int? id, int colorId, int materialId)
        {

            AppUser user = await _userManager.GetUserAsync(User);

            if (id == null) return BadRequest();

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            string cookieBasket = HttpContext.Request.Cookies["basket"];

            List<BasketVM> basketVMs = null;

            if (user != null)
            {
                Basket basketItem = await _context.Baskets.FirstOrDefaultAsync(b => b.AppUserId == user.Id && b.ProductId == product.Id && b.ColorId == colorId && b.MaterialId == materialId);

                if (basketItem == null)
                {
                    return NotFound();
                }

                _context.Baskets.Remove(basketItem);
                await _context.SaveChangesAsync();
            }

            if (!string.IsNullOrWhiteSpace(cookieBasket))
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(cookieBasket);

                BasketVM basketVM = basketVMs.FirstOrDefault(b => b.ProductId == id && b.ColorId == colorId && b.MaterialId == materialId);

                if (basketVM == null)
                {
                    return NotFound();
                }

                basketVMs.Remove(basketVM);
            }
            else
            {
                return BadRequest();
            }

            cookieBasket = JsonConvert.SerializeObject(basketVMs);
            HttpContext.Response.Cookies.Append("basket", cookieBasket);



            foreach (BasketVM compareVM in basketVMs)
            {
                Product dbProduct = await _context.Products
                    .Include(p => p.ProductColorMaterials)
                    .Include(p => p.ViewCount).FirstOrDefaultAsync(p => p.Id == compareVM.ProductId);

                compareVM.Price = dbProduct.DiscountPrice > 0 ? dbProduct.DiscountPrice : dbProduct.SalePrice;
                compareVM.Name = dbProduct.Name;
                compareVM.Image = dbProduct.ProductColorMaterials.FirstOrDefault(p => p.MaterialId == compareVM.MaterialId && p.ColorId == compareVM.ColorId)?.Image;




            }

            return PartialView("_BasketIndexPartial", basketVMs);
        }

    }
}
