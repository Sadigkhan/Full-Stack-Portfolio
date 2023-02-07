using JuanBackFinal.DAL;
using JuanBackFinal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using JuanBackFinal.ViewModels.Basket;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace JuanBackFinal.Controllers
{
    public class ProductController : Controller
    {
        private readonly JuanAppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ProductController(JuanAppDbContext context,UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> DetailModal(int? id)
        {
            if (id == null) return BadRequest();

            Product product = await _context.Products.Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (product == null) return NotFound();

            return PartialView("_ProductDetailPartial", product);
        }

        public async Task<IActionResult> AddBasket(int? id, int count = 1)
        {
            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            List<BasketVM> basketVMs = null;
            if (!User.Identity.IsAuthenticated)
            {
                string cookieBasket = HttpContext.Request.Cookies["basket"];

                if (cookieBasket != null)
                {
                    basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(cookieBasket);

                    if (basketVMs.Any(b => b.ProductId == id))
                    {
                        basketVMs.Find(b => b.ProductId == id).Count += count;
                    }
                    else
                    {


                        basketVMs.Add(new BasketVM
                        {
                            ProductId = (int)id,
                            Count = count
                        });
                    }
                }
                else
                {
                    basketVMs = new List<BasketVM>();


                    basketVMs.Add(new BasketVM
                    {
                        ProductId = (int)id,
                        Count = count
                    });
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
                List<Basket> baskets = _context.Baskets.Where(b=>b.AppUserId==user.Id).ToList();
                if (baskets==null)
                {

                    Basket NewBasket = new Basket
                    {
                        Product = product,
                        AppUserId = user.Id,
                        Count = 1,
                        
                    };
                    _context.Baskets.Add(NewBasket);
                    _context.SaveChanges();
                }
                else
                {
                    Basket basket = _context.Baskets.FirstOrDefault(b => b.AppUserId == user.Id&&b.ProductId==product.Id);
                    if (basket == null)
                    {
                        Basket NewBasket = new Basket
                        {
                            Product = product,
                            AppUserId = user.Id,
                            Count =count,

                        };
                        _context.Baskets.Add(NewBasket);
                        _context.SaveChanges();
                    }
                    else
                    {
                        basket.Count=count;
                        _context.SaveChanges();
                    }
                    List<Basket> baskets1 = _context.Baskets.Include(b=>b.Product).Where(b=>b.AppUserId==user.Id).ToList();
                    List<BasketVM> basketss = new List<BasketVM>();
                    foreach (var item in baskets1)
                    {
                        BasketVM basketVM = new BasketVM
                        {
                            Name=item.Product.Name,
                            Price=item.Product.SalePrice,
                            Count=item.Count,
                            Image=item.Product.MainImage,
                  
                        };
                       basketss.Add(basketVM);
                    }
                        return PartialView("_BasketPartial", basketss);
                }

            }
            _context.SaveChanges();

            return PartialView("_BasketPartial", basketVMs);
        }

        [HttpGet]
        public async Task<int> GetBasket()
        {
            List<BasketVM> basketVMs = null;
            if (!User.Identity.IsAuthenticated)
            {
                string cookieBasket = HttpContext.Request.Cookies["basket"];
                if (!string.IsNullOrWhiteSpace(cookieBasket))
                {
                    basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(cookieBasket);
                }
            }
            else
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                List<Basket> baskets = _context.Baskets.Where(b=>b.AppUserId==user.Id).ToList();
               return baskets.Count;
     
            }
            return basketVMs.Count();
        }

    }
}
