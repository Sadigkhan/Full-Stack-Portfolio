using DekorEvFinal.Helper;
using JuanBackFinal.DAL;
using JuanBackFinal.Extensions;
using JuanBackFinal.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JuanBackFinal.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ProductController : Controller
    {
        private readonly JuanAppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public ProductController(JuanAppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index(bool? status,int page=1)
        {
            ViewBag.Status = status;
            IEnumerable<Product>products =await  _context.Products
                .Include(p=>p.Category)
                .Where(t => status != null ? t.IsDeleted == status : !t.IsDeleted)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)products.Count() / 5);
            return View(products.Skip(page-1).Take(5));
        }
        public async Task<IActionResult> Detail(int? id, bool? status, int page = 1)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Product product = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.Category)
                .Include(p=>p.ProductColors).ThenInclude(p=>p.Color)
                .Include(p=>p.ProductSizes).ThenInclude(ps=>ps.Size)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product==null)
            {
                return NotFound();
            }
            return View(product);

        }

        public async Task<IActionResult> Create(bool? status, int page=1)
        {
            ViewBag.Categories = await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();
            ViewBag.Colors = await _context.Colors.Where(t => !t.IsDeleted).ToListAsync();
            ViewBag.Sizes = await _context.Sizes.Where(t => !t.IsDeleted).ToListAsync();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product,bool? status,int page=1)
        {
            ViewBag.Categories = await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();
            ViewBag.Colors = await _context.Colors.Where(t => !t.IsDeleted).ToListAsync();
            ViewBag.Sizes = await _context.Sizes.Where(t => !t.IsDeleted).ToListAsync();

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (product.ProductimageFiles.Count()>=6)
            {
                ModelState.AddModelError("ProductimageFiles", "Product Images can't be more than 5");
                return View();
            }
            if (!await _context.Categories.AnyAsync(c=>c.Id==product.CategoryId&&!c.IsDeleted))
            {
                ModelState.AddModelError("CategoryId","Please choose right category");
                return View();
            }
            if (product.ColorIds.Count > 0)
            {
                List<ProductColor> productColors = new List<ProductColor>();

                foreach (int item in product.ColorIds)
                {
                    if (!await _context.Colors.AnyAsync(t => t.Id != item && !t.IsDeleted))
                    {
                        ModelState.AddModelError("ColorIds", $"The color which you choose: {item} - is wrong");
                        return View();
                    }

                    ProductColor productColor = new ProductColor
                    {
                        ColorId = item
                    };

                    productColors.Add(productColor);
                }

                product.ProductColors = productColors;
            }

            if (product.SizeIds.Count > 0)
            {
                List<ProductSize> productSizes = new List<ProductSize>();

                foreach (int item in product.SizeIds)
                {
                    if (!await _context.Sizes.AnyAsync(t => t.Id != item && !t.IsDeleted))
                    {
                        ModelState.AddModelError("SizeIds", $"The size which you choose: {item} - is wrong");
                        return View();
                    }

                    ProductSize productSize = new ProductSize
                    {
                        SizeId = item
                    };

                    productSizes.Add(productSize);
                }

                product.ProductSizes = productSizes;
            }
            if (product.MainImageFile!=null)
            {
                if (!product.MainImageFile.CheckFileContentType("image/"))
                {
                    ModelState.AddModelError("MainImageFile", "Only image type is allowed");
                    return View();
                }
                if (!product.MainImageFile.CheckFileSize(100))
                {
                    ModelState.AddModelError("MainImageFile", "Image size can't be more than 100Kb");
                    return View();
                }
                product.MainImage = product.MainImageFile.CreateFile(_env, "assets", "img", "product");
            }
            else
            {
                ModelState.AddModelError("MainImageFile", "Main Image is required");
                return View();
            }
            if (product.ProductimageFiles.Count()>0)
            {
                List<ProductImage>productImages=new List<ProductImage>();

                foreach (IFormFile file in product.ProductimageFiles)
                {
                    if (!file.CheckFileContentType("image/"))
                    {
                        ModelState.AddModelError("ProductimageFiles","Only image type allowed");
                        return View();
                    }
                    if (!file.CheckFileSize(100))
                    {
                        ModelState.AddModelError("ProductimageFiles", "File size can't be  more than 100 Kb");
                        return View();
                    }
                    ProductImage productImage = new ProductImage
                    {
                        Image = file.CreateFile(_env, "assets", "img", "product"),
                        CreatedAt = DateTime.UtcNow.AddHours(4)
                    };

                    productImages.Add(productImage);
                }
                product.ProductImages = productImages;
            }
            else
            {
                ModelState.AddModelError("ProductimageFiles","Product images is required");
                return View();
            }

            product.IsAvailable=product.Count>0?true:false;
            product.CreatedAt=DateTime.UtcNow.AddHours(4);


            await _context.Products.AddAsync(product);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { status,page});
        }

        public async Task<IActionResult>Update(int?id,bool? status,int page = 1)
        {
            ViewBag.Categories = await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();
            ViewBag.Colors = await _context.Colors.Where(t => !t.IsDeleted).ToListAsync();
            ViewBag.Sizes = await _context.Sizes.Where(t => !t.IsDeleted).ToListAsync();

            if (id == null) return BadRequest();

            Product product = await _context.Products
                .Include(p => p.ProductColors).ThenInclude(pt => pt.Color)
                .Include(p => p.ProductImages)
                .Include(p=>p.ProductSizes).ThenInclude(pt=>pt.Size)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (product == null) return NotFound();

            product.ColorIds = product.ProductColors.Select(pt => pt.Color.Id).ToList();
            product.SizeIds = product.ProductSizes.Select(pt => pt.Size.Id).ToList();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(int? id, Product product, bool? status, int page = 1)
        {
            ViewBag.Categories = await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();
            ViewBag.Colors = await _context.Colors.Where(t => !t.IsDeleted).ToListAsync();
            ViewBag.Sizes = await _context.Sizes.Where(t => !t.IsDeleted).ToListAsync();

            if (id == null) return BadRequest();
            if (id!=product.Id)
            {
                return BadRequest();
            }
            Product dbProduct = await _context.Products
               .Include(p => p.ProductColors).ThenInclude(pt => pt.Color)
               .Include(p => p.ProductImages)
               .Include(p => p.ProductSizes).ThenInclude(pt => pt.Size)
               .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (dbProduct == null) return NotFound();

            if (!ModelState.IsValid) return View(dbProduct);

            int canuploadImage = 5 - (int)(dbProduct.ProductImages?.Where(p => !p.IsDeleted).Count());

            if (product.ProductimageFiles != null && product.ProductimageFiles.Length > canuploadImage)
            {
                ModelState.AddModelError("ProductImagesFile", $"Maximum uploadable image count {canuploadImage}");
                return View(dbProduct);
            }
            if (!await _context.Categories.AnyAsync(b => b.Id == product.CategoryId && !b.IsDeleted))
            {
                ModelState.AddModelError("CategoryId", "Please choose right category ");
                return View(dbProduct);
            }
            if (product.ColorIds.Count > 0)
            {
                _context.ProductColors.RemoveRange(dbProduct.ProductColors);

                List<ProductColor> productColors = new List<ProductColor>();

                foreach (int item in product.ColorIds)
                {
                    if (!await _context.Colors.AnyAsync(t => t.Id != item && !t.IsDeleted))
                    {
                        ModelState.AddModelError("ColorIds", $"Choosen Color Id {item} - is wrong");
                        return View(dbProduct);
                    }

                    ProductColor productColor = new ProductColor
                    {
                        ColorId = item
                    };

                    productColors.Add(productColor);
                }

                dbProduct.ProductColors = productColors;
            }
            else
            {
                _context.ProductColors.RemoveRange(dbProduct.ProductColors);
            }


            if (product.SizeIds.Count > 0)
            {
                _context.ProductSizes.RemoveRange(dbProduct.ProductSizes);

                List<ProductSize> productSizes = new List<ProductSize>();

                foreach (int item in product.SizeIds)
                {
                    if (!await _context.Sizes.AnyAsync(t => t.Id != item && !t.IsDeleted))
                    {
                        ModelState.AddModelError("SizeIds", $"Choosen Size Id {item} - is wrong");
                        return View(dbProduct);
                    }

                    ProductSize productSize = new ProductSize
                    {
                        SizeId = item
                    };

                    productSizes.Add(productSize);
                }

                dbProduct.ProductSizes = productSizes;
            }
            else
            {
                _context.ProductSizes.RemoveRange(dbProduct.ProductSizes);
            }

            if (product.MainImageFile != null)
            {
                if (!product.MainImageFile.CheckFileContentType("image/"))
                {
                    ModelState.AddModelError("MainImageFile", "Only image type is allowed");
                    return View();
                }
                if (!product.MainImageFile.CheckFileSize(100))
                {
                    ModelState.AddModelError("MainImageFile", "Image size can't be more than 100Kb");
                    return View();
                }
                Helper.DeleteFile(_env,dbProduct.MainImage, "assets", "img", "product");
                dbProduct.MainImage = product.MainImageFile.CreateFile(_env, "assets", "img", "product");
            }


            if (product.ProductimageFiles != null && product.ProductimageFiles.Count() > 0)
            {
                List<ProductImage> productImages = new List<ProductImage>();

                foreach (IFormFile file in product.ProductimageFiles)
                {
                    if (!file.CheckFileContentType("image/"))
                    {
                        ModelState.AddModelError("ProductimageFiles", "Only image type files allowed");
                        return View();
                    }

                    if (!file.CheckFileSize(100))
                    {
                        ModelState.AddModelError("ProductimageFiles", "File size can't be more than 100Kb");
                        return View();
                    }

                    ProductImage productImage = new ProductImage
                    {
                        Image = file.CreateFile(_env, "assets", "img", "product"),
                        CreatedAt = DateTime.UtcNow.AddHours(4)
                    };

                    dbProduct.ProductImages.Add(productImage);
                }
            }
            dbProduct.TopSeller = product.TopSeller;

            dbProduct.UpdatedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return RedirectToAction("index", new { status, page });

        }
         
        public async Task<IActionResult> DeleteImage(int? id)
        {
            ViewBag.Categories = await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();
            ViewBag.Colors = await _context.Colors.Where(t => !t.IsDeleted).ToListAsync();
            ViewBag.Sizes = await _context.Sizes.Where(t => !t.IsDeleted).ToListAsync();
            if (id == null)
            {
                return BadRequest();
            }
            Product dbProduct = await _context.Products
            .Include(p => p.ProductColors).ThenInclude(pt => pt.Color)
            .Include(p => p.ProductImages)
            .Include(p => p.Category)
            .Include(p => p.ProductSizes).ThenInclude(pt => pt.Size)
            .FirstOrDefaultAsync(p => p.ProductImages.Any(pi => pi.Id == id && !pi.IsDeleted));
            if (dbProduct == null)
            {
                return NotFound();
            }
            dbProduct.ProductImages.FirstOrDefault(p => p.Id == id).IsDeleted = true;
            dbProduct.ProductImages.FirstOrDefault(p => p.Id == id).DeletedAt = DateTime.UtcNow.AddHours(4);
            await _context.SaveChangesAsync();

            return PartialView("_ProductUpdateImagePartial", dbProduct);
        }

        public async Task<IActionResult> Delete(int? id, bool? status, int page = 1)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Product dbProduct = await _context.Products
        .Include(p => p.ProductColors).ThenInclude(pt => pt.Color)
        .Include(p => p.ProductImages)
        .Include(p => p.Category)
        .Include(p => p.ProductSizes).ThenInclude(pt => pt.Size)
        .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
            if (dbProduct == null)
            {
                return NotFound();
            }
            return View(dbProduct);
        }

        public async Task<IActionResult> DeleteProduct(int? id, bool? status, int page = 1)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Product dbProduct = await _context.Products
       .Include(p => p.ProductColors).ThenInclude(pt => pt.Color)
       .Include(p => p.ProductImages)
       .Include(p => p.Category)
       .Include(p => p.ProductSizes).ThenInclude(pt => pt.Size)
       .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
            if (dbProduct == null)
            {
                return NotFound();
            }
            dbProduct.IsDeleted = true;
            dbProduct.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return RedirectToAction("index", new { status, page });
        }

       
    }
}
