using DekorEvStartUpFinal.DAL;
using DekorEvStartUpFinal.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DekorEvStartUpFinal.Controllers
{
    public class HomeController : Controller
    {
        private readonly DekorEvStartupAppDbContext _context;
        public HomeController(DekorEvStartupAppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM
            {
                Categories = await _context.Categories.Where(c => !c.IsDeleted && c.IsMain).ToListAsync(),
                IsNew = await _context.Products
                .Include(p=>p.ProductColorMaterials).ThenInclude(p=>p.Color)
                .Include(p=>p.ProductColorMaterials).ThenInclude(p=>p.Material)
                .Where(p=>!p.IsDeleted&&p.IsNew && !p.DeletedByAdmin).ToListAsync(),
                IsVip=await _context.Products
                .Include(p => p.ProductColorMaterials).ThenInclude(p => p.Color)
                .Include(p => p.ProductColorMaterials).ThenInclude(p => p.Material)
                .Where(p=>!p.IsDeleted&&p.IsVip && !p.DeletedByAdmin).ToListAsync(),
                IsPremium = await _context.Products
                .Include(p => p.ProductColorMaterials).ThenInclude(p => p.Color)
                .Include(p => p.ProductColorMaterials).ThenInclude(p => p.Material)
                .Where(p => !p.IsDeleted && p.IsPremium && !p.DeletedByAdmin).ToListAsync()
            };
            return View(homeVM);
        }
    }
}
