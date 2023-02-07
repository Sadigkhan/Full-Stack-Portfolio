using DekorEvStartUpFinal.DAL;
using DekorEvStartUpFinal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DekorEvStartUpFinal.Controllers
{
    public class ShopController : Controller
    {
        private readonly DekorEvStartupAppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ShopController(DekorEvStartupAppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Index(int? CatId,/*int sortId,*/ List<int> categoryIds, List<int> colourIds,List<int>materialIds)
        {
            

            if (CatId != null)
            {
                if (!_context.Categories.Any(c => c.Id == CatId))
                {
                    return NotFound();
                }
                else
                {
                    ViewBag.CategoryName = _context.Categories.FirstOrDefault(c => c.Id == CatId).Name;
                    ViewBag.RelatedSubCategories = _context.Categories.Where(c => !c.IsMain && c.ParentId == CatId).ToList();
                    ViewBag.RelatedProducts = _context.Products.Where(p => p.Category.ParentId == CatId&&!p.IsDeleted&&!p.DeletedByAdmin).Include(p => p.ProductColorMaterials).ThenInclude(p => p.Color)
                .Include(p => p.ProductColorMaterials).ThenInclude(p => p.Material).ToList();
                    ViewBag.RelatedProductsCount = _context.Products.Where(p => p.Category.ParentId == CatId && !p.IsDeleted && !p.DeletedByAdmin).ToList().Count();
                }
                   
            }
          


            //ViewBag.CurrentPage = page;
            //ViewBag.TotalPage = Math.Ceiling((decimal)_context.Products.Count()/4);

            IQueryable<Product> query = _context.Products.
                Include(b => b.Category).Where(c => !c.IsDeleted  && !c.DeletedByAdmin).
                Include(p => p.ProductColorMaterials).ThenInclude(p => p.Color).
                Include(p => p.ProductColorMaterials).ThenInclude(p => p.Material).Where(m => !m.IsDeleted);


            List<Product> products = new List<Product>();

            if ((categoryIds != null && categoryIds.Count > 0) || (colourIds != null && colourIds.Count > 0) || (materialIds != null && materialIds.Count > 0 ))
            {
                
                if (categoryIds.Count != 0)
                {
                    foreach (int categoryId in categoryIds)
                    {
                        
                        products.AddRange(query.Where(p => p.CategoryId == categoryId));

                    }
                }

                if (colourIds.Count != 0)
                {
           
                    foreach (int colourId in colourIds)
                    {
                        if (categoryIds.Count != 0)
                        {
                    
                            products = products.FindAll(p => p.ProductColorMaterials.Any(pm => pm.ColorId == colourId));
                       
                        }
                        else
                        {
                            products.AddRange(query.Where(p => p.ProductColorMaterials.Any(pm => pm.ColorId == colourId)));
                        }

                    }
                }
                if (materialIds.Count != 0)
                {
         
                    foreach (int materialId in materialIds)
                    {
                        if (categoryIds.Count != 0 || colourIds.Count!=0)
                        {
               
                            products = products.FindAll(p => p.ProductColorMaterials.Any(pm => pm.MaterialId == materialId));
                      
                        }
                        else
                        {
                            products.AddRange(query.Where(p => p.ProductColorMaterials.Any(pm => pm.MaterialId == materialId)));
                        }

                    }
                }

            }
            else
            {
                products.AddRange(query);
            }




            ViewBag.Categories = _context.Categories.Where(c => !c.IsDeleted && c.IsMain).Include(c => c.Products).ToList();
            ViewBag.SubCategories = _context.Categories.Where(c => !c.IsDeleted && !c.IsMain).Include(c => c.Products).ToList();
            ViewBag.Materials = _context.Materials.Include(m => m.ProductColorMaterials).ThenInclude(m => m.Product).Where(p => !p.IsDeleted).ToList();
            ViewBag.Colors = _context.Colors.Where(mc => !mc.IsDeleted).Include(m => m.ProductColorMaterials).ThenInclude(m => m.Product).Where(p => !p.IsDeleted).ToList();
            //ViewBag.Id = sortId;

            //switch (sortId)
            //{
            //    case 1:
            //        products= _context.Products.Where(p=>p.DiscountPrice>0).ToList();
            //        break;
            //        case 2:
            //        products= _context.Products.Where(p=>p.IsNew).ToList();
            //        break;

            //    default:
            //        break;
            //}

            return View(products);
        }
    }
}
