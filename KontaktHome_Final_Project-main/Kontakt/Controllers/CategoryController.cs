using Kontakt.DAL;
using Kontakt.Models;
using Kontakt.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontakt.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetMainCategory()
        {

            List<Category> categories = await _context.Categories
                .Where(x => !x.IsDeleted &&x.IsMain).OrderBy(x => x.CreatedAt).ToListAsync();
            return PartialView("_GetMainCtgPartial", categories);

        }
        public async Task<IActionResult> GetSubCategory(int? id,string keyMob,string keyMetod,string keyfrom)
        {
            if (id==null)
            {
                return BadRequest();
            }
            List<Category> categories = await _context.Categories
                .Where(x => !x.IsDeleted && x.ParentId == id).OrderBy(x => x.CreatedAt).ToListAsync();
            ViewBag.PerntId = id;
            if (keyMetod=="over")
            {
                if (keyfrom=="home")
                {
                    return PartialView("_GetSubCtgHomePartial", categories);
                }
                else
                {
                    return PartialView("_GetSubCtgPartial", categories);
                }
               
              
                    
                 
                
            }
            else
            {
                if (keyMob=="mobil")
                {
                    return PartialView("_GetMobilSubCtgPartial", categories);
                }
                else
                {
                    Category category = await _context.Categories
                        .Include(x => x.Parent)
                        .Include(x => x.Children)
                        .Where(x => x.Id == id).FirstOrDefaultAsync();
                    List<Category> categorieslast = await _context.Categories
                        .Include(x => x.Children)
                        .Include(x=>x.Products)
                        .Include(x => x.CategoryBrands).ThenInclude(x => x.Brand)
                        .Where(x => x.ParentId == id &&!x.IsDeleted).ToListAsync();

                   

                    if (category==null || categorieslast == null)
                    {
                        return NotFound();
                    }
                    List<Product> products = await _context.Products
                       .Include(x => x.ProductDetails).ThenInclude(x => x.DetailKey).ThenInclude(x => x.DetailValues)
                       .Where(x => x.Category.ParentId == category.Id).ToListAsync();
                    CategoryVM categoryVM = new CategoryVM
                    {
                        Category = category,
                        Categories = categorieslast,
                        Products=products

                    };

                    return View(categoryVM);
                }
                
            }
            
            

        }
        public async Task<IActionResult> GetBrand(int? id, string keyMob, string keyMetod)
        {
            List<Category> categories = await _context.Categories
                .Where(x => !x.IsDeleted && x.ParentId == id).OrderBy(x => x.CreatedAt).ToListAsync();

            List<CategoryBrand> categoryBrands = await _context.CategoryBrands
                .Include(x=>x.Category)
                .Include(x=>x.Brand)
                .Where(x => !x.IsDeleted && x.CategoryId==id).OrderBy(x => x.CreatedAt).ToListAsync();
            ViewBag.PerntId = id;
           

            if (keyMetod=="over")
            {
                if (categories.Count()>0)
                {
                    return PartialView("_GetChildCtg", categories);
                    
                }
                else
                {
                    return PartialView("_GetBrandPartial", categoryBrands);
                }
                
                
            }
            else
            {
                if (keyMob=="mobil")
                {
                    if (categories.Count() > 0)
                    {
                        return PartialView("_GetChildCtgMob", categories);

                    }
                    else
                    {
                        return PartialView("_GetBrandMobilPartial", categoryBrands);
                    }
                    
                }
                else
                {
                    return View(categoryBrands);
                }
                
            }
            

        }

        
    }
}
