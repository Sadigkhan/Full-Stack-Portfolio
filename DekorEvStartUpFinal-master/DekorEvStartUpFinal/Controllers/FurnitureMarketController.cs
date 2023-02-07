using DekorEvStartUpFinal.DAL;
using DekorEvStartUpFinal.Models;
using DekorEvStartUpFinal.ViewModels.Market;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DekorEvStartUpFinal.Controllers
{
    public class FurnitureMarketController : Controller
    {
        private readonly DekorEvStartupAppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public FurnitureMarketController(DekorEvStartupAppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            List<AppUser> appUser= await _context.Users.Where(u=>u.isMarket && u.EmailConfirmed==true&&u.isConfirmed).Include(u=>u.Products).ToListAsync();
            return View(appUser);
        }

        public async Task<IActionResult> StoreDetail(string? id)
        {
            AppUser currentUser = await _userManager.GetUserAsync(User);
            AppUser appUser = await _userManager.FindByIdAsync(id);
            if (appUser == null) return NotFound();
            ViewCount viewCount = await _context.ViewCounts.FirstOrDefaultAsync(v=>v.AppUserId == id);


            if (currentUser!=null)
            {
                if (currentUser.Id != id)
                {
                    if (viewCount == null)
                    {
                        ViewCount newViewCount = new ViewCount();
                        newViewCount.AppUserId = id;
                        newViewCount.Count = 1;
                        await _context.ViewCounts.AddAsync(newViewCount);
                    }
                    else
                    {
                        viewCount.Count++;
                    }
                }

            }
            else
            {
                if (viewCount == null)
                {
                    ViewCount newViewCount = new ViewCount();
                    newViewCount.AppUserId = id;
                    newViewCount.Count = 1;
                    await _context.ViewCounts.AddAsync(newViewCount);
                }
                else
                {
                    viewCount.Count++;
                }
            }

            await _context.SaveChangesAsync();

            List<Product> products = await _context.Products.Where(p => p.AppUserId == appUser.Id &&!p.IsDeleted&&!p.DeletedByAdmin)
                .Include(p=>p.Category)
                .Include(p=>p.ProductColorMaterials).ThenInclude(p=>p.Color)
                .Include(p => p.ProductColorMaterials).ThenInclude(p => p.Material).
                ToListAsync();



            MarketDetailVM marketDetailVM = new MarketDetailVM
            {
                User = appUser,
                Products = products,
                ViewCounts = await _context.ViewCounts.FirstOrDefaultAsync(v => v.AppUserId == id)
            };

           

            return View(marketDetailVM);
        }
    }
}
