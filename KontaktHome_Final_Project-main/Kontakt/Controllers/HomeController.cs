using Kontakt.DAL;
using Kontakt.Models;
using Kontakt.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontakt.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult>  Index(ResetPasswordVM reset)
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

            

            List<Product> Disproducts = new List<Product>();
            List<Product> proList = new List<Product>();

            List<Product> products = await _context.Products
                .Include(p => p.ProductImages)
                .Include(x => x.Reviews)
                .Include(x => x.ProductDetails).ThenInclude(x => x.DetailKey).ThenInclude(x => x.DetailValues)
                .Where(p => !p.IsDeleted && p.DiscountPrice>0).OrderBy(x => x.CreatedAt).ToListAsync();

            List<Category> categories = await _context.Categories
                .Include(x => x.Children).ThenInclude(x => x.Products)
                .Include(x => x.Parent)
                .Include(x => x.Products).ThenInclude(x => x.Category)
                .Where(x => !x.IsDeleted).ToListAsync();
            
            foreach (Product product in products)
            {
                
                    if (proList.Where(x=>x.CategoryId==product.CategoryId).Count()==0)
                    {
                        if ((100 - ((product.DiscountPrice / product.Price) * 100) > 0))
                        {
                            proList.Add(product);
                        }
                        
                    }
                    else
                    {
                        if ((100 - ((product.DiscountPrice / product.Price) * 100) > (100 - ((proList.LastOrDefault(p=>p.CategoryId==product.CategoryId).DiscountPrice / proList.LastOrDefault(p => p.CategoryId == product.CategoryId).Price) * 100))))
                        {
                            proList.Remove(proList.LastOrDefault(p => p.CategoryId == product.CategoryId));
                            proList.Add(product);
                        }
                    }

            }
            Disproducts.AddRange(proList.OrderByDescending(x=> 100 - (x.DiscountPrice / x.Price) * 100).Take(4));
            ViewBag.DisProCount = proList.Count();
            HomeVM homeVM = new HomeVM()
            {

                Products = await _context.Products
                .Include(p => p.ProductImages)
                .Include(x => x.Reviews)
                .Include(x => x.ProductDetails).ThenInclude(x => x.DetailKey).ThenInclude(x => x.DetailValues)
                .Where(p => !p.IsDeleted).OrderBy(x => x.CreatedAt).Take(4).ToListAsync(),
                Brands = await _context.Brands.Where(b => !b.IsDeleted).ToListAsync(),
                Categories = await _context.Categories
                .Include(x => x.Children).ThenInclude(x => x.Products)
                .Include(x => x.Parent)
                .Include(x => x.Products).ThenInclude(x => x.Category)
                .Where(x => !x.IsDeleted).ToListAsync(),
                CategoryBrands = await _context.CategoryBrands.Where(x => !x.IsDeleted).ToListAsync(),
                DisProducts = Disproducts,
                WishVMs = wishVMs
                
                
              

            };
            if (reset.Id!= null)
            {
                ViewBag.reset = "true";
                ViewBag.id = reset.Id;
                ViewBag.token = reset.Token;
            }
            
            return View(homeVM);
        }

        
    }
}
