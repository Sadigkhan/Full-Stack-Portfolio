using DekorEvStartUpFinal.DAL;
using DekorEvStartUpFinal.Models;
using DekorEvStartUpFinal.ViewModels.Basket;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using DekorEvStartUpFinal.ViewModels.Compare;

namespace DekorEvStartUpFinal.Services
{
    public class LayoutService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DekorEvStartupAppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public LayoutService(IHttpContextAccessor httpContextAccessor, DekorEvStartupAppDbContext context, UserManager<AppUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _userManager = userManager;
        }

        public async Task<Setting> GetSetting()
        {
            return await _context.Settings.FirstOrDefaultAsync();
        }

        public async Task<List<Category>> GetCategory()
        {
            return await _context.Categories.Where(c=>c.IsMain&&!c.IsDeleted).ToListAsync();
        }

        public async Task<int> BasketItemCount()
        {
            AppUser user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            int count = 0;
            if (user != null)
            {
                count = await _context.Baskets.Where(u => u.AppUserId == user.Id).CountAsync();

                return count;
            }
            else
            {
                string cookieBasket = _httpContextAccessor.HttpContext.Request.Cookies["basket"];

                if (!string.IsNullOrWhiteSpace(cookieBasket))
                {
                    List<BasketVM> basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(cookieBasket);

                    count = basketVMs.Count();
                    return count;
                }
                else
                {
                    return 0;
                }
            }



           
        }


        public async Task<int> CompareItemCount()
        {
           
            int count = 0;
           
        
                string cookieCompare = _httpContextAccessor.HttpContext.Request.Cookies["compare"];

                if (!string.IsNullOrWhiteSpace(cookieCompare))
                {
                    List<CompareVM> compareVMs = JsonConvert.DeserializeObject<List<CompareVM>>(cookieCompare);

                    count = compareVMs.Count();
                    return count;
                }
                else
                {
                    return 0;
                }




        }
    }
}
