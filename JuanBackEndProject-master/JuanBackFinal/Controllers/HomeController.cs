using JuanBackFinal.DAL;
using JuanBackFinal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace JuanBackFinal.Controllers
{
    public class HomeController : Controller
    {
        private readonly JuanAppDbContext _context;

        public HomeController(JuanAppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Sliders=_context.Sliders,
                ServiceOffers=_context.ServiceOffers,
                Products=_context.Products.Where(p=>!p.IsDeleted),
                Banners=_context.Banners.Where(p => !p.IsDeleted),
                Blogs=_context.Blogs.Where(p => !p.IsDeleted).Include(b => b.Publisher),
                Brands=_context.Brands.Where(p => !p.IsDeleted),
                Settings=await  _context.Settings.FirstOrDefaultAsync(),
                Categories=_context.Categories.Where(p => !p.IsDeleted),
            };
            return View(homeVM);
        }
    }
}
