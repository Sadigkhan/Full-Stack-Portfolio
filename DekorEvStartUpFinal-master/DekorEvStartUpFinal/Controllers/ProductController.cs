using DekorEvStartUpFinal.DAL;
using DekorEvStartUpFinal.Models;
using DekorEvStartUpFinal.ViewModels.Basket;
using DekorEvStartUpFinal.ViewModels.Compare;
using DekorEvStartUpFinal.ViewModels.ProductVM;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DekorEvStartUpFinal.Controllers
{
    public class ProductController : Controller
    {
        private readonly DekorEvStartupAppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public ProductController(DekorEvStartupAppDbContext context,IWebHostEnvironment env, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _env = env;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Index(int sortId, int page=1)
        {

            IEnumerable<Product> products = await _context.Products.
                Where(p => !p.IsDeleted).
                Include(p => p.Category).Where(c => !c.IsDeleted).
                Include(p => p.ProductColorMaterials).ThenInclude(p => p.Material).Where(m => !m.IsDeleted).
                Include(p => p.ProductColorMaterials).ThenInclude(p => p.Color).Where(c => !c.IsDeleted).
                ToListAsync();
            return View();
        }

        public async Task<IActionResult> ProductDetail(int? id, int colorId, int materialId)
        {
            //ViewBag.BrowserName = Request.HttpContext.Connection.RemoteIpAddress;
            ViewBag.ColorId = colorId;
            ViewBag.MaterialId = materialId;
            if (id == null) return BadRequest();
            Product product = await _context.Products
                .Include(x=>x.ViewCount)
                .Include(p => p.AppUser)
                .Include(x => x.ProductImages)
                .Include(x => x.Category)
                .Include(x => x.ProductColorMaterials).ThenInclude(x => x.Color)
                .Include(x => x.ProductColorMaterials).ThenInclude(x => x.Material)
                //.Include(x => x.Reviews).ThenInclude(c => c.AppUser)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted && x.ProductColorMaterials.Any(pc=>pc.ColorId == colorId && pc.MaterialId == materialId));
            if (product == null) return NotFound();

            ProductDetailVM productDetailVM = new ProductDetailVM();
            ViewCount viewCount = await _context.ViewCounts.FirstOrDefaultAsync(v => v.ProductId == id);

            AppUser currentuser = await _userManager.GetUserAsync(User);
            if (currentuser!=null)
            {
                if (product.AppUserId != currentuser.Id)
                {
                    if (viewCount == null)
                    {
                        ViewCount newViewCount = new ViewCount();
                        newViewCount.ProductId = id;
                        newViewCount.Count = 1;
                        await _context.ViewCounts.AddAsync(newViewCount);
                    }
                    else
                    {
                        viewCount.Count++;
                    }

                    await _context.SaveChangesAsync();
                }


            }
            else
            {
                if (viewCount == null)
                {
                    ViewCount newViewCount = new ViewCount();
                    newViewCount.ProductId = id;
                    newViewCount.Count = 1;
                    await _context.ViewCounts.AddAsync(newViewCount);
                }
                else
                {
                    viewCount.Count++;
                }
                await _context.SaveChangesAsync();
            }

            productDetailVM.Product = product;
            if (User.Identity.Name != null)
            {
                productDetailVM.appUser = await _userManager.Users.Where(x => x.NormalizedUserName == User.Identity.Name.ToUpperInvariant()).FirstOrDefaultAsync();
            }
            //productDetailVM.Review = new Review { ProductId = id };




            productDetailVM.Products = _context.Products.Include(p => p.Category).Where(x => x.CategoryId == product.CategoryId).Take(6).ToList();
            productDetailVM.ViewCounts = _context.ViewCounts.FirstOrDefault(v => v.ProductId == id);

            ViewBag.ColorId = colorId;
            ViewBag.MaterialId = materialId;


            return View(productDetailVM);
        }

        public async Task<IActionResult> AddBasket(int? id, string page, int colorId, int materialId)
        {
            AppUser user = await _userManager.GetUserAsync(User);

            if (id == null) return BadRequest();

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();
           
            if (user != null)
            {
                List<Basket> existedBasket = await _context.Baskets
                    .Where(b => b.AppUserId == user.Id).ToListAsync();
                if (existedBasket.Any(b => b.ProductId == product.Id && b.ColorId == colorId && b.MaterialId == materialId))
                {
                    existedBasket.FirstOrDefault(b => b.ProductId == product.Id && b.ColorId == colorId && b.MaterialId == materialId).Count++;
                }
                else
                {
                    Basket basket = new Basket
                    {
                        AppUserId = user.Id,
                        ProductId = product.Id,
                        ColorId = colorId,
                        MaterialId = materialId,
                        Count = 1,
                        CreatedAt = System.DateTime.UtcNow.AddHours(4)
                    };

                    await _context.Baskets.AddAsync(basket);
                }
                await _context.SaveChangesAsync();
            }

            List<BasketVM> basketVMs = null;
            string cookieBasket = HttpContext.Request.Cookies["basket"];

            if (!string.IsNullOrWhiteSpace(cookieBasket))
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(cookieBasket);

                if (basketVMs.Any(b => b.ProductId == id && colorId == b.ColorId && materialId == b.MaterialId))
                {
                    basketVMs.Find(b => b.ProductId == id && colorId == b.ColorId && materialId == b.MaterialId).Count++;
                }
                else
                {
                    basketVMs.Add(new BasketVM
                    {
                        ProductId = (int)id,
                        Count = 1,
                        ColorId = colorId,
                        MaterialId = materialId,
                    });
                }
            }
            else
            {
                basketVMs = new List<BasketVM>();

                basketVMs.Add(new BasketVM
                {
                    ProductId = (int)id,
                    Count = 1,
                    ColorId = colorId,
                    MaterialId = materialId
                });
            }

            foreach (BasketVM basketVM in basketVMs)
            {

                Product dbProduct = await _context.Products
                    .FirstOrDefaultAsync(p => p.Id == basketVM.ProductId);
                basketVM.Image = dbProduct.MainImage;
                basketVM.Price = (double)(dbProduct.DiscountPrice > 0 ? dbProduct.DiscountPrice : dbProduct.SalePrice);
                basketVM.Name = dbProduct.Name;

            }

            cookieBasket = JsonConvert.SerializeObject(basketVMs);
            HttpContext.Response.Cookies.Append("basket", cookieBasket);

            return Json(new { count = basketVMs.Count});
        }

        public async Task<IActionResult> Compare(int? id, string page, int colorId, int materialId)
        {

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            List<CompareVM> compareVMs = null;
            string cookieCompare = HttpContext.Request.Cookies["compare"];

            if (!string.IsNullOrWhiteSpace(cookieCompare))
            {
                compareVMs = JsonConvert.DeserializeObject<List<CompareVM>>(cookieCompare);
                if (compareVMs.Count >= 3)
                {
                    return Json(new { status = 121212, count = compareVMs.Count });
                }

                if (compareVMs.Any(b => b.ProductId == id && colorId == b.ColorId && materialId == b.MaterialId))
                {
                    return Json(new { status = 215, count = compareVMs.Count });
                }
                else
                {
                    compareVMs.Add(new CompareVM
                    {
                        ProductId = (int)id,
                        ColorId = colorId,
                        MaterialId = materialId
                    });
                }
            }
            else
            {
                compareVMs = new List<CompareVM>();

                compareVMs.Add(new CompareVM
                {
                    ProductId = (int)id,
                    ColorId = colorId,
                    MaterialId = materialId
                });
            }

            foreach (CompareVM compareVM in compareVMs)
            {

                Product dbProduct = await _context.Products
                    .FirstOrDefaultAsync(p => p.Id == compareVM.ProductId);
                compareVM.Image = dbProduct.MainImage;
                compareVM.Price = (double)(dbProduct.DiscountPrice > 0 ? dbProduct.DiscountPrice : dbProduct.SalePrice);
                compareVM.Name = dbProduct.Name;
            }

            cookieCompare = JsonConvert.SerializeObject(compareVMs);
            HttpContext.Response.Cookies.Append("compare", cookieCompare);

            return Json(new { status = 201 ,count= compareVMs.Count});
        }

        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return RedirectToAction("Index", "Product");
            }
            List<Product> products = await _context.Products.Where(p => p.Name.ToLower().Contains(query.ToLower())).Include(p=>p.ProductColorMaterials).ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> SearchPartial(string query)
        {
            List<Product> products = await _context.Products.Where(p => p.Name.ToLower().Contains(query.ToLower())).Include(p => p.ProductColorMaterials).ToListAsync();
            return PartialView("_ProductSearchPartial", products);
        }

        // Making VIP Fronted And Premium From Detail Page


        //public async Task<IActionResult> MakeFrontedFromDetail(int? id)
        //{

        //    if (id == null)
        //    {
        //        return BadRequest();
        //    }
        //    Product dbProduct = await _context.Products.FirstOrDefaultAsync(c => c.Id == id);
        //    if (dbProduct == null) return NotFound();

        //    dbProduct.IsFronted = true;
        //    dbProduct.FrontedPaymentDate = DateTime.UtcNow.AddHours(4);
        //    await _context.SaveChangesAsync();
        //    return View(dbProduct);
        //}
        //public async Task<IActionResult> MakeVipFromDetail(int? id)
        //{

        //    if (id == null)
        //    {
        //        return BadRequest();
        //    }
        //    Product dbProduct = await _context.Products.FirstOrDefaultAsync(c => c.Id == id);
        //    if (dbProduct == null) return NotFound();

        //    dbProduct.IsFronted = true;
        //    dbProduct.IsVip = true;
        //    dbProduct.VipPaymentDate = DateTime.UtcNow.AddHours(4);
        //    await _context.SaveChangesAsync();
        //    return View(dbProduct);

        //}
        //public async Task<IActionResult> MakePremiumFromDetail(int? id)
        //{

        //    if (id == null)
        //    {
        //        return BadRequest();
        //    }
        //    Product dbProduct = await _context.Products.FirstOrDefaultAsync(c => c.Id == id);
        //    if (dbProduct == null) return NotFound();

        //    dbProduct.IsFronted = true;
        //    dbProduct.IsVip = true;
        //    dbProduct.IsPremium = true;
        //    dbProduct.PremiumPaymentDate = DateTime.UtcNow.AddHours(4);
        //    await _context.SaveChangesAsync();
        //    return View(dbProduct);

        //}
    }
}
