using DekorEvStartUpFinal.DAL;
using DekorEvStartUpFinal.Extensions;
using DekorEvStartUpFinal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DekorEvStartUpFinal.Areas.Market.Controllers
{
    [Area("Market")]
    [Authorize(Roles = "Market")]
    public class ProductController : Controller
    {
        private readonly DekorEvStartupAppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<AppUser> _userManager;
        public ProductController(DekorEvStartupAppDbContext context, IWebHostEnvironment env, UserManager<AppUser> userManager)
        {
            _context = context;
            _env = env;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(bool? status/*,bool?isVip,bool?isPremium,bool?isFronted*/, int page = 1)
        {
            AppUser user= await _userManager.GetUserAsync(User);
            IEnumerable<Product> products = await _context.Products
                .Where(p => p.AppUser.NormalizedUserName ==user.NormalizedUserName&&!p.IsDeleted)
                .Include(p => p.Category)
                .Include(p => p.ProductColorMaterials)
                //.Where(p => status != null ? p.IsDeleted == status : !p.IsDeleted)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)products.Count() / 5);


            return View(products.Skip((page - 1) * 5).Take(5));
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.Where(c => !c.IsDeleted && c.IsMain).ToListAsync();
            ViewBag.Colors = await _context.Colors.Where(c => !c.IsDeleted).ToListAsync();
            ViewBag.MaTerials = await _context.Materials.Where(c => !c.IsDeleted).ToListAsync();
            return View();
        }

        public async Task<IActionResult> GetSub(int? subId)
        {
            if (subId == null)
            {
                return BadRequest();
            }

            if (!await _context.Categories.AnyAsync(c => c.Id == subId))
            {
                return NotFound();
            }

            List<Category> categories = await _context.Categories.Where(c => !c.IsDeleted && !c.IsMain && c.ParentId == subId).ToListAsync();
            return PartialView("_GetSubPartial", categories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product? product, bool? status, int page = 1)
        {
            ViewBag.Categories = await _context.Categories.Where(c => !c.IsDeleted && c.IsMain).ToListAsync();
            ViewBag.Colors = await _context.Colors.Where(c => !c.IsDeleted).ToListAsync();
            ViewBag.MaTerials = await _context.Materials.Where(c => !c.IsDeleted).ToListAsync();

            AppUser appUser = null;
            if (User.Identity.IsAuthenticated)
            {
                appUser = await _context.Users.Include(u => u.Products).FirstOrDefaultAsync(u => u.NormalizedUserName == User.Identity.Name.ToUpperInvariant() && !u.isAdmin && u.isMarket);
            }

            if (appUser == null) return BadRequest();

            if (!ModelState.IsValid)
            {
                return View(product);
            }
            product.Name = product.Name.Trim();
            product.Description = product.Description.Trim();
            product.CostPrice = product.CostPrice;
            product.DiscountPrice = product.DiscountPrice;
            product.SalePrice = product.SalePrice;
            product.IsNew = true;
            product.IsDeliverable = product.IsDeliverable;
            product.IsVip = false;
            product.ProductCode = product.CategoryId.ToString() + product.Name.Substring(0, 2).ToUpper() + DateTime.UtcNow.AddHours(4).ToString();
            product.IsFronted = false;
            product.MaterialIds = product.MaterialIds;
            product.ColorIds = product.ColorIds;
            product.AppUserId = appUser.Id;

            Regex regex = new Regex(@"\s{2,}");
            if (regex.IsMatch(product.Name) && regex.IsMatch(product.Description))
            {
                ModelState.AddModelError("Name", "Can't be empty");
                ModelState.AddModelError("Description", "Can't be empty");
                return View();
            }


            if (product.MaterialIds.Count != product.ColorIds.Count || product.MaterialIds.Count != product.Counts.Count || product.Counts.Count != product.ColorIds.Count)
            {
                ModelState.AddModelError("", "Incorect");
                return View();
            }

            foreach (int item in product.MaterialIds)
            {
                if (!await _context.Materials.AnyAsync(s => s.Id == item))
                {
                    ModelState.AddModelError("", "Incorect Material Id");
                    return View();
                }
            }

            foreach (int item in product.ColorIds)
            {
                if (!await _context.Colors.AnyAsync(s => s.Id == item))
                {
                    ModelState.AddModelError("", "Incorect Color Id");
                    return View();
                }
            }
            List<ProductColorMaterial> productColorMaterials = new List<ProductColorMaterial>();

            for (int i = 0; i < product.ColorIds.Count; i++)
            {
                if (product.ImageFiles[i] != null)
                {
                    if (!product.ImageFiles[i].CheckFileContentType("image/"))
                    {
                        ModelState.AddModelError("ImageFiles", "Please Choose image type file");
                        return View(product);
                    }
                    if (!product.ImageFiles[i].CheckFileSize(250))
                    {
                        ModelState.AddModelError("ImageFiles", "File size must be less than 250 KB");
                        return View(product);
                    }
                }
                else
                {
                    ModelState.AddModelError("ImageFiles", "Product's main image can't be empty");
                    return View(product);
                }

                ProductColorMaterial productColorMaterial = new ProductColorMaterial
                {
                    ColorId = product.ColorIds[i],
                    MaterialId = product.MaterialIds[i],
                    Count = product.Counts[i],
                    Image = product.ImageFiles[i].CreateFile(_env, "assets", "images")
                };

                productColorMaterials.Add(productColorMaterial);
            }


            product.ProductColorMaterials = productColorMaterials;

            if (!await _context.Categories.AnyAsync(b => b.Id == product.CategoryId && !b.IsDeleted))
            {
                ModelState.AddModelError("CategoryId", "Choose the correct category");
                return View(product);
            }

            if (!await _context.Categories.AnyAsync(p => p.Id == product.CategoryId && !p.IsDeleted))
            {
                ModelState.AddModelError("CategoryId", "Please choose correct category");
                return View(product);
            }


            product.IsAvailable = product.Count > 0 ? true : false;
            product.CreatedAt = DateTime.UtcNow.AddHours(4);
            product.MainImage = productColorMaterials[0].Image;
            product.Count = product.Counts.Sum();
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { status, page });

        }
        public async Task<IActionResult> Update(int? id, bool? status, int page = 1)
        {
            ViewBag.Categories = await _context.Categories.Where(c => !c.IsDeleted && c.IsMain).ToListAsync();
            ViewBag.Colors = await _context.Colors.Where(c => !c.IsDeleted).ToListAsync();
            ViewBag.Materials = await _context.Materials.Where(c => !c.IsDeleted).ToListAsync();
            if (id == null) return BadRequest();

            Product product = await _context.Products
                .Include(P => P.Category)
                .Include(p => p.ProductColorMaterials).ThenInclude(cm => cm.Material)
                .Include(p => p.ProductColorMaterials).ThenInclude(cm => cm.Color)
                .Include(p => p.ProductImages)
                .Include(p => p.AppUser)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (product?.AppUser?.NormalizedUserName != User.Identity.Name.ToUpperInvariant())
            {
                return NotFound();
            }

            ViewBag.SubCategories = await _context.Categories.Where(c => !c.IsDeleted && !c.IsMain && c.ParentId == product.Category.ParentId).ToListAsync();


            return View(product);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Product product, int[] WhichImg, bool? status, int page = 1)
        {
            ViewBag.Categories = await _context.Categories.Where(c => !c.IsDeleted && c.IsMain).ToListAsync();
            ViewBag.Colors = await _context.Colors.Where(c => !c.IsDeleted).ToListAsync();
            ViewBag.Materials = await _context.Materials.Where(c => !c.IsDeleted).ToListAsync();
            if (id == null)
            {
                return BadRequest();
            }
            if (id != product.Id)
            {
                return BadRequest();
            }

            Product dbProduct = await _context.Products
                  .Include(p => p.Category)
                  .Include(p => p.ProductImages)
                  .Include(p => p.ProductColorMaterials).ThenInclude(p => p.Color)
                  .Include(p => p.ProductColorMaterials).ThenInclude(p => p.Material)
                  .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (dbProduct == null) return NotFound();

            //if (!ModelState.IsValid) return View(dbProduct);

            ViewBag.SubCategories = await _context.Categories.Where(c => !c.IsDeleted && !c.IsMain && c.ParentId == dbProduct.Category.ParentId).ToListAsync();

            product.Name = product.Name.Trim();
            product.Description = product.Description.Trim();


            if (product.MaterialIds.Count != product.ColorIds.Count || product.MaterialIds.Count != product.Counts.Count || product.Counts.Count != product.ColorIds.Count)
            {
                ModelState.AddModelError("", "Incorect");
                return View(dbProduct);
            }



            foreach (int item in product.MaterialIds)
            {
                if (!await _context.Materials.AnyAsync(s => s.Id == item))
                {
                    ModelState.AddModelError("", "Incorect Material Id");
                    return View(dbProduct);
                }
            }



            foreach (int item in product.ColorIds)
            {
                if (!await _context.Colors.AnyAsync(s => s.Id == item))
                {
                    ModelState.AddModelError("", "Incorect Color Id");
                    return View(dbProduct);
                }
            }
            int fileCount = 0;
            int wImg = 0;
            int ferq = 0;
            _context.ProductColorMaterials.RemoveRange(dbProduct.ProductColorMaterials);
            List<ProductColorMaterial> productColorMaterials = new List<ProductColorMaterial>();

            for (int i = 0; i < product.ColorIds.Count; i++)
            {
                ProductColorMaterial productColorMaterial = new ProductColorMaterial
                {
                    ColorId = product.ColorIds[i],
                    MaterialId = product.MaterialIds[i],
                    Count = product.Counts[i],
                    Image = dbProduct.ProductColorMaterials.Any(p => p.ColorId == product.ColorIds[i] && p.MaterialId == product.MaterialIds[i]) ? dbProduct.ProductColorMaterials.Find(p => p.ColorId == product.ColorIds[i] && p.MaterialId == product.MaterialIds[i]).Image : ""

                };
                product.Count += product.Counts[i];


                productColorMaterials.Add(productColorMaterial);
                Regex regex = new Regex(@"\s{2,}");
                if (regex.IsMatch(product.Name) && regex.IsMatch(product.Description))
                {
                    ModelState.AddModelError("Name", "Should not be Space");
                    ModelState.AddModelError("Description", "Should not be Space");
                    return View(dbProduct);
                }



            }

            for (int i = 0; i < productColorMaterials.Count(); i++)
            {
                for (int a = 1; a < productColorMaterials.Count() - i; a++)
                {
                    if (productColorMaterials[i].ColorId == productColorMaterials[i + a].ColorId && productColorMaterials[i].MaterialId == productColorMaterials[i + a].MaterialId)
                    {
                        ModelState.AddModelError("", "2 eyni deyer olmaz");
                        return View(dbProduct);
                    }
                }
            }

            ferq = (productColorMaterials.Count() - dbProduct.ProductColorMaterials.Count()) >= 0 ?
               (productColorMaterials.Count() - dbProduct.ProductColorMaterials.Count()) :
               (dbProduct.ProductColorMaterials.Count() - productColorMaterials.Count());

            for (int i = 0; i < product.ColorIds.Count(); i++)
            {
                if (product.ImageFiles.Count() > 0)
                {
                    for (int f = fileCount; f < product.ImageFiles.Count(); f++)
                    {
                        if (WhichImg.Length > 0)
                        {
                            for (int w = wImg; w < WhichImg.Length; w++)
                            {
                                productColorMaterials[WhichImg[w] - ferq].Image = product.ImageFiles[f].CreateFile(_env, "assets", "images");
                                wImg++;
                                break;
                            }
                            fileCount++;
                            break;
                        }
                        else
                        {
                            if (dbProduct.ProductColorMaterials.Count() > productColorMaterials.Count())
                            {
                                productColorMaterials[dbProduct.ProductColorMaterials.Count() - ferq - 1 + i].Image = product.ImageFiles[f].CreateFile(_env, "assets", "images");
                                fileCount++;
                                break;
                            }
                            else
                            {
                                productColorMaterials[dbProduct.ProductColorMaterials.Count() + i].Image = product.ImageFiles[f].CreateFile(_env, "assets", "images");
                                fileCount++;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    productColorMaterials[i].Image = dbProduct.ProductColorMaterials[i].Image;
                }
            }
            for (int m = 0; m < product.MaterialIds.Count(); m++)
            {
                if(WhichImg != null && WhichImg.Count() > 0)
                {
                    if(WhichImg.Any(p=>p == m))
                    {

                    }
                }
            }



            dbProduct.ProductColorMaterials = productColorMaterials;
            dbProduct.MainImage = productColorMaterials[0].Image;
            dbProduct.CategoryId = product.CategoryId;
            dbProduct.Name = product.Name;
            dbProduct.CostPrice = product.CostPrice;
            dbProduct.SalePrice = product.SalePrice;
            dbProduct.DiscountPrice = product.DiscountPrice;
            dbProduct.Description = product.Description;
            dbProduct.Count = product.Counts.Sum();
            dbProduct.IsAvailable = product.Count > 0 ? true : false;
            dbProduct.UpdatedAt = DateTime.UtcNow.AddHours(4);
            //dbProduct.IsPremium = product.IsPremium;


            await _context.SaveChangesAsync();

            return RedirectToAction("index", new { status, page });
        }

        public async Task<IActionResult> Detail(int? id, bool? status, int page = 1)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Product product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Include(p => p.ProductColorMaterials).ThenInclude(p => p.Color)
                .Include(p => p.ProductColorMaterials).ThenInclude(p => p.Material)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            return View(product);

        }

        public async Task<IActionResult> DeleteImage(int? id)
        {
            ViewBag.Categories = await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();
            ViewBag.Colors = await _context.Colors.Where(c => !c.IsDeleted).ToListAsync();
            ViewBag.Materials = await _context.Materials.Where(c => !c.IsDeleted).ToListAsync();
            if (id == null)
            {
                return BadRequest();
            }
            Product product = await _context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.ProductImages.Any(pi => pi.Id == id && !pi.IsDeleted));
            if (product == null)
            {
                return NotFound();
            }
            product.ProductImages.FirstOrDefault(p => p.Id == id).IsDeleted = true;
            product.ProductImages.FirstOrDefault(p => p.Id == id).DeletedAt = DateTime.UtcNow.AddHours(4);
            await _context.SaveChangesAsync();

            return PartialView("_ProductUpdateImagePartial", product);
        }

        public async Task<IActionResult> Delete(int? id, bool? status, int page = 1)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Product product = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductColorMaterials).ThenInclude(p => p.Material)
                .Include(p => p.ProductColorMaterials).ThenInclude(p => p.Color)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public async Task<IActionResult> DeleteProduct(int? id, bool? status, int page = 1)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Product product = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.Category)
                .Include(p => p.ProductColorMaterials).ThenInclude(p => p.Material)
                .Include(p => p.ProductColorMaterials).ThenInclude(p => p.Color)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (product == null)
            {
                return NotFound();
            }

            product.IsDeleted = true;
            product.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return RedirectToAction("index", "Product", new { status, page });
        }

        public async Task<IActionResult> GetFormColoRMaterialCount()
        {
            ViewBag.Colors = await _context.Colors.ToListAsync();
            ViewBag.Materials = await _context.Materials.ToListAsync();

            return PartialView("_ProductColorMaterialPartial");
        }



        public async Task<IActionResult> MakeVip(int? id, int page = 1)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Product dbProduct = await _context.Products.FirstOrDefaultAsync(c => c.Id == id);
            if (dbProduct == null) return NotFound();
            dbProduct.IsFronted = true;
            dbProduct.IsVip = true;
            dbProduct.VipPaymentDate = DateTime.UtcNow.AddHours(4);
            await _context.SaveChangesAsync();
            //ViewBag.Status = status;

            IEnumerable<Product> products = await _context.Products
                 .Where(p => p.AppUser.NormalizedUserName == User.Identity.Name.ToUpperInvariant())
                .Include(p=>p.Category)
                .Include(s => s.ProductColorMaterials)
                //.Where(s => status != null ? s.IsVip == status : true)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)products.Count() / 5);

            return PartialView("_ProductIndexPartial", products.Skip((page - 1) * 5).Take(5));
        }

        public async Task<IActionResult> MakePremium(int? id, bool status, int page = 1)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Product dbProduct = await _context.Products.FirstOrDefaultAsync(c => c.Id == id);
            if (dbProduct == null) return NotFound();
            dbProduct.IsFronted = true;
            dbProduct.IsVip = true;
            dbProduct.IsPremium = true;
            dbProduct.PremiumPaymentDate = DateTime.UtcNow.AddHours(4);
            await _context.SaveChangesAsync();
            ViewBag.Status = status;

            IEnumerable<Product> products = await _context.Products
                 .Where(p => p.AppUser.NormalizedUserName == User.Identity.Name.ToUpperInvariant())
                .Include(p => p.Category)
                .Include(s => s.ProductColorMaterials)
                //.Where(s => status != null ? s.IsPremium == status : true)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)products.Count() / 5);

            return PartialView("_ProductIndexPartial", products.Skip((page - 1) * 5).Take(5));
        }




        public async Task<IActionResult> MakeFronted(int? id, bool status, int page = 1)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Product dbProduct = await _context.Products.FirstOrDefaultAsync(c => c.Id == id);
            if (dbProduct == null) return NotFound();

            dbProduct.IsFronted = true;
            dbProduct.FrontedPaymentDate = DateTime.UtcNow.AddHours(4);
            await _context.SaveChangesAsync();
            ViewBag.Status = status;

            IEnumerable<Product> products = await _context.Products
                 .Where(p => p.AppUser.NormalizedUserName == User.Identity.Name.ToUpperInvariant())
                .Include(p => p.Category)
                .Include(s => s.ProductColorMaterials)
                //.Where(s => status != null ? s.IsPremium == status : true)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)products.Count() / 5);

            return PartialView("_ProductIndexPartial", products.Skip((page - 1) * 5).Take(5));
        }

    }
}
