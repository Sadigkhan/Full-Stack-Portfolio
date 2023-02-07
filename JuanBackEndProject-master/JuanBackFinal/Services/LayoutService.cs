using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using JuanBackFinal.DAL;
using JuanBackFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JuanBackFinal.ViewModels.Basket;
using Microsoft.AspNetCore.Identity;

namespace JuanBackFinal.Services
{
    public class LayoutService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JuanAppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public LayoutService(IHttpContextAccessor httpContextAccessor, JuanAppDbContext context,UserManager<AppUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<BasketVM>> GetBasket()
        {
            List<BasketVM> basketVMs = null;
            if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {

            string cookieBasket = _httpContextAccessor.HttpContext.Request.Cookies["basket"];


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
                basketVM.Price = dbProduct.DiscountPrice > 0 ? dbProduct.DiscountPrice : dbProduct.SalePrice;
                //basketVM.ExTax = dbProduct.ExTax;
                 basketVM.Name = dbProduct.Name;
            }
            }
            else
            {
                AppUser user = await _userManager.FindByNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
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

                    };
                    basketss.Add(basketVM);
                }
                return basketss;
            }

            return basketVMs;
        }

        public async Task<Setting> GetSetting()
        {
            
            return await _context.Settings.FirstOrDefaultAsync();

        }
        public async Task<List<Category>> GetCategory()
        {
           List<Category>  category = await _context.Categories.Where(c=>!c.IsDeleted).ToListAsync();
            return category;
        }
    }
}
