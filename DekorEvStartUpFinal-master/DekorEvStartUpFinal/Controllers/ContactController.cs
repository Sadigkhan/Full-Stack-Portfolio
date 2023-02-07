using DekorEvStartUpFinal.DAL;
using DekorEvStartUpFinal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DekorEvStartUpFinal.Controllers
{
    public class ContactController : Controller
    {
        private readonly DekorEvStartupAppDbContext _context;
        public ContactController(DekorEvStartupAppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            Setting contact = await _context.Settings.FirstOrDefaultAsync();
            return View(contact);
        }
    }
}
