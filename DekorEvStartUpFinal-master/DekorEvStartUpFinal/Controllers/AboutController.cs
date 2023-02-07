using DekorEvStartUpFinal.DAL;
using DekorEvStartUpFinal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DekorEvStartUpFinal.Controllers
{
    public class AboutController : Controller
    {
        public readonly DekorEvStartupAppDbContext _context;
        public AboutController(DekorEvStartupAppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            Setting about =await  _context.Settings.FirstOrDefaultAsync();
            return View(about);
        }
    }
}
