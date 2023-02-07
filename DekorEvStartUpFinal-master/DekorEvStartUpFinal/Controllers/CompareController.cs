using DekorEvStartUpFinal.DAL;
using DekorEvStartUpFinal.Models;
using DekorEvStartUpFinal.ViewModels.Compare;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DekorEvStartUpFinal.Controllers
{
    public class CompareController : Controller
    {

        private readonly DekorEvStartupAppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public CompareController(DekorEvStartupAppDbContext context, UserManager<AppUser> userManager)
        {

            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult>  Index()
        {
            List<CompareVM> compareVMs = new List<CompareVM>();

            string cookieCompare = HttpContext.Request.Cookies["compare"];

            if (!string.IsNullOrWhiteSpace(cookieCompare))
            {
                compareVMs = JsonConvert.DeserializeObject<List<CompareVM>>(cookieCompare);
            }
            else
            {
                compareVMs = new List<CompareVM>();
            }
            
            foreach (CompareVM compareVM in compareVMs)
            {
                Product dbProduct = await _context.Products.Include(p=>p.ProductColorMaterials).Include(p => p.ViewCount).FirstOrDefaultAsync(p => p.Id == compareVM.ProductId);

                compareVM.Price = dbProduct.DiscountPrice > 0 ? dbProduct.DiscountPrice : dbProduct.SalePrice;
                compareVM.Name = dbProduct.Name;
                compareVM.Description = dbProduct.Description;
                compareVM.ProductCount = dbProduct.Count;
                compareVM.Image = dbProduct.ProductColorMaterials?.FirstOrDefault(p => p.MaterialId == compareVM.MaterialId && p.ColorId == compareVM.ColorId)?.Image;

              
                if (dbProduct.ViewCount!=null)
                {
                    compareVM.Count = dbProduct.ViewCount.Count;

                }
                else
                {
                    compareVM.Count = 0;

                }
            }

            return View(compareVMs);
        }




        public async Task<IActionResult> DeleteCompare(int? id, int colorId, int materialId)
        {
            string cookieBasket = HttpContext.Request.Cookies["compare"];

            List<CompareVM> compareVMs = null;

            if (!string.IsNullOrWhiteSpace(cookieBasket))
            {
                compareVMs = JsonConvert.DeserializeObject<List<CompareVM>>(cookieBasket);

                CompareVM compareVM = compareVMs.FirstOrDefault(b => b.ProductId == id && b.ColorId == colorId && b.MaterialId == materialId);

                if (compareVM == null)
                {
                    return NotFound();
                }

                compareVMs.Remove(compareVM);
            }
            else
            {
                return BadRequest();
            }

            cookieBasket = JsonConvert.SerializeObject(compareVMs);
            HttpContext.Response.Cookies.Append("compare", cookieBasket);

            foreach (CompareVM compareVM in compareVMs)
            {
                Product dbProduct = await _context.Products.Include(p => p.ProductColorMaterials).Include(p => p.ViewCount).FirstOrDefaultAsync(p => p.Id == compareVM.ProductId);

                compareVM.Price = dbProduct.DiscountPrice > 0 ? dbProduct.DiscountPrice : dbProduct.SalePrice;
                compareVM.Name = dbProduct.Name;
                compareVM.Description = dbProduct.Description;
                compareVM.ProductCount = dbProduct.Count;
                compareVM.Image = dbProduct.ProductColorMaterials?.FirstOrDefault(p => p.MaterialId == compareVM.MaterialId && p.ColorId == compareVM.ColorId)?.Image;


                if (dbProduct.ViewCount != null)
                {
                    compareVM.Count = dbProduct.ViewCount.Count;

                }
                else
                {
                    compareVM.Count = 0;

                }
            }


            return PartialView("_CompareIndexPartial", compareVMs);
        }
    }
}
