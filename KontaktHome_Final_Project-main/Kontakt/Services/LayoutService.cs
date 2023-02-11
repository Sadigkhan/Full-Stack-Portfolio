using Kontakt.DAL;
using Kontakt.Models;
using Kontakt.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontakt.Services
{
    
        public class LayoutService
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly AppDbContext _context;

            public LayoutService(IHttpContextAccessor httpContextAccessor, AppDbContext context)
            {
                _httpContextAccessor = httpContextAccessor;
                _context = context;
            }

            public async Task<List<Category>> GetCategory()
            {


                List<Category> categories = await _context.Categories
                .Include(x=>x.CategoryBrands).ThenInclude(x=>x.Brand)
                .Include(x=>x.Children)
                .ToListAsync();

                return categories;
            }
        public async Task<List<BasketVM>> GetBasket()
        {
            string cookieBasket = _httpContextAccessor.HttpContext.Request.Cookies["basket"];

            List<BasketVM> basketVMs = null;

            if (!string.IsNullOrWhiteSpace(cookieBasket))
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
                basketVM.Price = (double)(dbProduct.DiscountPrice > 0 ? dbProduct.DiscountPrice : dbProduct.Price);
                basketVM.Title = dbProduct.Title;

            }

            return basketVMs;
        }

        public async Task<List<WishVM>> GetWish()
        {
            string cookieWish = _httpContextAccessor.HttpContext.Request.Cookies["Wish"];

            List<WishVM> wishVMs = null;

            if (!string.IsNullOrWhiteSpace(cookieWish))
            {
                wishVMs = JsonConvert.DeserializeObject<List<WishVM>>(cookieWish);
            }
            else
            {
                wishVMs = new List<WishVM>();
            }

            foreach (WishVM wishVM in wishVMs)
            {
                Product dbProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == wishVM.ProductId);
                wishVM.Image = dbProduct.MainImage;
                wishVM.Price = (double)(dbProduct.DiscountPrice > 0 ? dbProduct.DiscountPrice : dbProduct.Price);
                wishVM.Title = dbProduct.Title;

            }

            return wishVMs;
        }


    }
    
}
