using Kontakt.DAL;
using Kontakt.Extensions;
using Kontakt.Helpers;
using Kontakt.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontakt.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Publisher,Manager")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(bool? status, int page = 1)
        {
            ViewBag.Status = status;

            IQueryable<Product> products = _context.Products
                .Include(x => x.ProductDetails).ThenInclude(x => x.DetailKey).ThenInclude(x=>x.DetailValues)
                .Include(x=>x.Brand)
                .Include(x=>x.Category).ThenInclude(x=>x.CategoryDetailKeys)
                .Include(x=>x.ProductTags).ThenInclude(x=>x.Tag)
                .OrderByDescending(c => c.CreatedAt)
                .AsQueryable();

            if (status != null)
                products = products.Where(c => c.IsDeleted == status);

            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)products.Count() / 5);
            return View(products.Skip((page - 1) * 5).Take(5).ToList());
        }
        public async Task<IActionResult> Create()
        {
            
            ViewBag.Brands = await _context.Brands.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(c => !c.IsDeleted && c.IsMain).ToListAsync();
            ViewBag.Tags = await _context.Tags.Where(t => !t.IsDeleted).ToListAsync();
            return View();
        }

        public async Task<IActionResult> GetBrand(int? id)
        {

            List<CategoryBrand> brands = await _context.CategoryBrands
                .Include(x=>x.Brand)
                .Include(x=>x.Category)
                .Where(x => x.CategoryId == id &&!x.IsDeleted).OrderByDescending(x=>x.Id).ToListAsync();

            return PartialView("_GetBrandPartial",brands);

        }

        public async Task<IActionResult> GetSimilarProduct(int? id, int? ctgId)
        {

            List<Product> products = await _context.Products
                .Include(x => x.Brand)
                .Include(x => x.Category)
                .Include(x=>x.ProductDetails).ThenInclude(x=>x.DetailKey).ThenInclude(x=>x.DetailValues)
                .Where(x => x.BrandId == id && !x.IsDeleted &&x.CategoryId==ctgId).OrderByDescending(x => x.Id).ToListAsync();

            return PartialView("_SimilarProList", products);

        }

        public async Task<IActionResult> GetSimilarDeatil(int? id)
        {

            Product product = await _context.Products
                .Include(p => p.ProductTags).ThenInclude(pt => pt.Tag)
                .Include(p => p.ProductDetails).ThenInclude(x => x.DetailKey).ThenInclude(x => x.DetailValues)
                .Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.Id == id);


            ViewBag.Details = await _context.CategoryDetailKeys
                .Include(x => x.DetailKey).ThenInclude(x => x.DetailValues)/*.ThenInclude(x => x.Brand)*/.Where(x => x.CategoryId == product.CategoryId)
                .Where(x => x.CategoryId == product.CategoryId && !x.DetailKey.IsDeleted).OrderBy(x => x.Id).ToListAsync();
            ViewBag.BrandId = product.BrandId;

            ViewBag.ProDetail = await _context.ProductDetails
                .Include(x => x.Product)
                .Include(x => x.DetailKey)
                .Include(x => x.DetailValue)
                .Where(x => x.ProductId == product.Id)
                .ToListAsync();

            return PartialView("_SimilarProDetsil", product);

        }
        public async Task<IActionResult> GetDetail(int? id, int? brandId)
        {

            List<CategoryDetailKey> categoryDetailKeys = await _context.CategoryDetailKeys
                .Include(x => x.DetailKey).ThenInclude(x=>x.DetailValues)/*.ThenInclude(x=>x.Brand)*/.Where(x=>x.CategoryId==id)
                .Where(x => x.CategoryId == id &&!x.DetailKey.IsDeleted).OrderBy(x => x.Id).ToListAsync();
            ViewBag.BrandId = brandId;
            return PartialView("_GetDetailPartial", categoryDetailKeys);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, bool? status, int? ProductId, int page = 1)
        {
            ViewBag.Brands = await _context.Brands.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Categories = await _context.Categories.Where(b => !b.IsDeleted && b.IsMain).ToListAsync();
            ViewBag.Tags = await _context.Tags.Where(t => !t.IsDeleted).ToListAsync();

            if (!ModelState.IsValid)
            {
                return View();
            }
           
            

            if (!await _context.Brands.AnyAsync(b => b.Id == product.BrandId && !b.IsDeleted))
            {
                ModelState.AddModelError("BrandId", "Duzgun Brand Secin");
                return View();
            }

            if (!await _context.Categories.AnyAsync(b => b.Id == product.CategoryId && !b.IsDeleted))
            {
                ModelState.AddModelError("CategoryId", "Duzgun Category Secin ");
                return View(product);
            }

            

            if (product.TagIds.Count > 0)
            {
                List<ProductTag> productTags = new List<ProductTag>();

                foreach (int item in product.TagIds)
                {
                    if (!await _context.Tags.AnyAsync(t => t.Id != item && !t.IsDeleted))
                    {
                        ModelState.AddModelError("TagIds", $"Secilen Id {item} - li Tag Yanlisdir");
                        return View();
                    }

                    ProductTag productTag = new ProductTag
                    {
                        TagId = item
                    };

                    productTags.Add(productTag);
                }

                product.ProductTags = productTags;
            }
            if (ProductId != null)
            {
                if (product.MainImageFile != null)
                {
                    if (!product.MainImageFile.CheckFileContentType("image/jpeg"))
                    {
                        ModelState.AddModelError("MainImageFile", "Secilen Seklin Novu Uygun deyil");
                        return View();
                    }

                    if (!product.MainImageFile.CheckFileSize(3000))
                    {
                        ModelState.AddModelError("MainImageFile", "Secilen Seklin Olcusu Maksimum 300 Kb Ola Biler");
                        return View();
                    }

                    product.MainImage = product.MainImageFile.CreateFile(_env, "user", "assets", "img", "ProductImg");
                }
                else
                {
                    Product DBproduct = await _context.Products.Where(x => x.Id == ProductId).FirstOrDefaultAsync();
                    product.MainImage = DBproduct.MainImage;
                }
            }
            else
            {
                if (product.MainImageFile != null)
                {
                    if (!product.MainImageFile.CheckFileContentType("image/jpeg"))
                    {
                        ModelState.AddModelError("MainImageFile", "Secilen Seklin Novu Uygun deyil");
                        return View();
                    }

                    if (!product.MainImageFile.CheckFileSize(3000))
                    {
                        ModelState.AddModelError("MainImageFile", "Secilen Seklin Olcusu Maksimum 300 Kb Ola Biler");
                        return View();
                    }

                    product.MainImage = product.MainImageFile.CreateFile(_env, "user", "assets", "img", "ProductImg");
                }
                else
                {
                    ModelState.AddModelError("MainImageFile", "Main Sekil Mutleq Secilmelidir");
                    return View(product);
                }
            }


            if (ProductId != null)
            {
                if (product.ProductImagesFile!=null)
                {
                    if (product.ProductImagesFile.Count() > 5)
                    {
                        ModelState.AddModelError("ProductImagesFile", "Maksimum 5 sekil secile biler");
                        return View(product);
                    }
                    else
                    {
                        List<ProductImage> productImages = new List<ProductImage>();

                        foreach (IFormFile file in product.ProductImagesFile)
                        {
                            if (!file.CheckFileContentType("image/jpeg"))
                            {
                                ModelState.AddModelError("ProductImagesFile", "Secilen Seklin Novu Uygun deyil");
                                return View(product);
                            }

                            if (!file.CheckFileSize(3000))
                            {
                                ModelState.AddModelError("ProductImagesFile", "Secilen Seklin Olcusu Maksimum 300 Kb Ola Biler");
                                return View(product);
                            }

                            ProductImage productImage = new ProductImage
                            {
                                Image = file.CreateFile(_env, "user", "assets", "img", "ProductImg"),
                                CreatedAt = DateTime.UtcNow.AddHours(4)
                            };

                            productImages.Add(productImage);
                        }

                        product.ProductImages = productImages;
                    }

                }
                else
                {

                    List<ProductImage> DBproductImages = await _context.ProductImages.Where(x => x.ProductId == ProductId).ToListAsync();
                    List<ProductImage> NEWproductImages = new List<ProductImage>();

                    foreach (ProductImage DBproductImage in DBproductImages)
                    {
                        ProductImage productImage = new ProductImage
                        {
                            Image = DBproductImage.Image,
                            CreatedAt = DateTime.UtcNow.AddHours(4)
                        };
                        NEWproductImages.Add(productImage);
                    }

                    product.ProductImages=(NEWproductImages);
                }
            }
            else
            {

                if (product.ProductImagesFile == null)
                {
                    ModelState.AddModelError("ProductImagesFile", "Sekil secilmeyib minimum 1 sekil secilmelidir");
                    return View();
                }
                if (product.ProductImagesFile.Count() > 6)
                {
                    ModelState.AddModelError("ProductImagesFile", $"maksimum yukleyebileceyin say 6");
                    return View();
                }

                if (product.ProductImagesFile.Count() > 0)
                {
                    if (product.ProductImagesFile.Count() > 5)
                    {
                        ModelState.AddModelError("ProductImagesFile", "Maksimum 5 sekil secile biler");
                        return View(product);
                    }
                    else
                    {
                        List<ProductImage> productImages = new List<ProductImage>();

                        foreach (IFormFile file in product.ProductImagesFile)
                        {
                            if (!file.CheckFileContentType("image/jpeg"))
                            {
                                ModelState.AddModelError("ProductImagesFile", "Secilen Seklin Novu Uygun deyil");
                                return View(product);
                            }

                            if (!file.CheckFileSize(3000))
                            {
                                ModelState.AddModelError("ProductImagesFile", "Secilen Seklin Olcusu Maksimum 300 Kb Ola Biler");
                                return View(product);
                            }

                            ProductImage productImage = new ProductImage
                            {
                                Image = file.CreateFile(_env, "user", "assets", "img", "ProductImg"),
                                CreatedAt = DateTime.UtcNow.AddHours(4)
                            };

                            productImages.Add(productImage);
                        }

                        product.ProductImages = productImages;
                    }

                }
                else
                {
                    ModelState.AddModelError("ProductImagesFile", "Product Images File Sekil Mutleq Secilmelidir");
                    return View(product);
                }
            }


           

            product.Availability = product.Count > 0 ? true : false;
            product.CreatedAt = DateTime.UtcNow.AddHours(4);

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            if (product.DetailIds.Count>0)
            {
                List<ProductDetail> productDetails = new List<ProductDetail>();
                Product dbProduct = await _context.Products.Where(x=>x.Id==product.Id).FirstOrDefaultAsync();

                if (dbProduct!=null)
                {
                    foreach (var item in product.DetailIds)
                    {
                        DetailValue detailValue = await _context.DetailValues.FirstOrDefaultAsync(x => x.Id == item);

                        if (detailValue != null)
                        {
                            ProductDetail productDetail = new ProductDetail
                            {
                                DetailKeyId = detailValue.DetailKeyId,
                                DetailValueId = detailValue.Id,
                                ProductId = dbProduct.Id

                            };
                            productDetails.Add(productDetail);
                        }
                    }
                    await _context.ProductDetails.AddRangeAsync(productDetails);
                    await _context.SaveChangesAsync();
                }
                
            }
            
            return RedirectToAction("index", new { status, page });
        }
        public async Task<IActionResult> Detail(int? id, bool? status, int page = 1)
        {
            if (id == null) return BadRequest();

            Product product = await _context.Products
                .Include(p => p.ProductTags).ThenInclude(pt => pt.Tag)
                .Include(p => p.ProductImages)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p=>p.ProductDetails).ThenInclude(x=>x.DetailKey)
                .Include(x=>x.ProductDetails).ThenInclude(x=>x.DetailValue)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            return View(product);
        }

        public async Task<IActionResult> Update(int? id, bool? status, int page = 1)
        {
           
            ViewBag.Categories = await _context.Categories.Where(c => !c.IsDeleted && !c.IsMain).ToListAsync();
            ViewBag.Tags = await _context.Tags.Where(t => !t.IsDeleted).ToListAsync();

            if (id == null) return BadRequest();

            Product product = await _context.Products
                .Include(p => p.ProductTags).ThenInclude(pt => pt.Tag)
                .Include(p=>p.ProductDetails).ThenInclude(x=>x.DetailKey).ThenInclude(x=>x.DetailValues)
                .Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();
            ViewBag.Brands = await _context.CategoryBrands
                .Include(x => x.Brand)
                .Include(x => x.Category)
                .Where(x => x.CategoryId == product.CategoryId && !x.IsDeleted).OrderByDescending(x => x.Id).ToListAsync();


            ViewBag.Details= await _context.CategoryDetailKeys
                .Include(x => x.DetailKey).ThenInclude(x => x.DetailValues)/*.ThenInclude(x => x.Brand)*/.Where(x => x.CategoryId == product.CategoryId)
                .Where(x => x.CategoryId == product.CategoryId &&!x.DetailKey.IsDeleted).OrderBy(x => x.Id).ToListAsync();
            ViewBag.BrandId = product.BrandId;

            ViewBag.ProDetail = await _context.ProductDetails
                .Include(x => x.Product)
                .Include(x=>x.DetailKey)
                .Include(x=>x.DetailValue)
                .Where(x=>x.ProductId==product.Id)
                .ToListAsync();

            product.TagIds = product.ProductTags.Select(pt => pt.Tag.Id).ToList();
            

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Product product, bool? status, int page = 1)
        {
            
            ViewBag.Categories = await _context.Categories.Where(c => !c.IsDeleted && !c.IsMain).ToListAsync();
            ViewBag.Tags = await _context.Tags.Where(t => !t.IsDeleted).ToListAsync();

            if (id == null) return BadRequest();

            if (id != product.Id) return BadRequest();

            Product dbProduct = await _context.Products
                .Include(p => p.ProductTags).ThenInclude(pt => pt.Tag)
                .Include(p => p.ProductImages)
                .Include(p=>p.ProductDetails).ThenInclude(x=>x.DetailKey).ThenInclude(x=>x.DetailValues)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (dbProduct == null) return NotFound();
            ViewBag.Brands = await _context.CategoryBrands
                .Include(x => x.Brand)
                .Include(x => x.Category)
                .Where(x => x.CategoryId == dbProduct.CategoryId).OrderByDescending(x => x.Id).ToListAsync();

            ViewBag.Details = await _context.CategoryDetailKeys
                .Include(x => x.DetailKey).ThenInclude(x => x.DetailValues)/*.ThenInclude(x => x.Brand)*/.Where(x => x.CategoryId == product.CategoryId)
                .Where(x => x.CategoryId == product.CategoryId).OrderBy(x => x.Id).ToListAsync();
            ViewBag.BrandId = product.BrandId;

            ViewBag.ProDetail = await _context.ProductDetails
                .Include(x => x.Product)
                .Include(x => x.DetailKey)
                .Include(x => x.DetailValue)
                .Where(x => x.ProductId == product.Id)
                .ToListAsync();

            if (!ModelState.IsValid) return View(dbProduct);

            int canuploadimage = 5 - (int)(dbProduct.ProductImages?.Where(p => !p.IsDeleted).Count());

            if (product.ProductImagesFile != null && product.ProductImagesFile.Length > canuploadimage)
            {
                ModelState.AddModelError("ProductImagesFile", $"maksimum yukleyebileceyin say {canuploadimage}");
                return View(dbProduct);
            }

            if (product.BrandId==null)
            {
                ModelState.AddModelError("BrandId", "Duzgun Brand Secin");
                return View(dbProduct);
            }

            if (!await _context.Brands.AnyAsync(b => b.Id == product.BrandId && !b.IsDeleted))
            {
                ModelState.AddModelError("BrandId", "Duzgun Brand Secin");
                return View(dbProduct);
            }

            if (!await _context.Categories.AnyAsync(b => b.Id == product.CategoryId && !b.IsDeleted))
            {
                ModelState.AddModelError("CategoryId", "Duzgun Category Secin ");
                return View(dbProduct);
            }



            if (product.TagIds.Count > 0)
            {
                _context.ProductTags.RemoveRange(dbProduct.ProductTags);

                List<ProductTag> productTags = new List<ProductTag>();

                foreach (int item in product.TagIds)
                {
                    if (!await _context.Tags.AnyAsync(t => t.Id != item && !t.IsDeleted))
                    {
                        ModelState.AddModelError("TagIds", $"Secilen Id {item} - li Tag Yanlisdir");
                        return View(product);
                    }

                    ProductTag productTag = new ProductTag
                    {
                        TagId = item
                    };

                    productTags.Add(productTag);
                }

                dbProduct.ProductTags = productTags;
            }
            else
            {
                _context.ProductTags.RemoveRange(dbProduct.ProductTags);
            }




            if (product.MainImageFile != null)
            {
                if (!product.MainImageFile.CheckFileContentType("image/jpeg"))
                {
                    ModelState.AddModelError("MainImageFile", "Secilen Seklin Novu Uygun");
                    return View(dbProduct);
                }

                if (!product.MainImageFile.CheckFileSize(300))
                {
                    ModelState.AddModelError("MainImageFile", "Secilen Seklin Olcusu Maksimum 300 Kb Ola Biler");
                    return View(dbProduct);
                }
                if (dbProduct.MainImage!=null)
                {
                    Helper.DeleteFile(_env, dbProduct.MainImage, "user", "assets", "img", "ProductImg");
                }
                

                dbProduct.MainImage = product.MainImageFile.CreateFile(_env, "user", "assets", "img", "ProductImg");
            }


            if (product.ProductImagesFile != null && product.ProductImagesFile.Count() > 0)
            {
                List<ProductImage> productImages = new List<ProductImage>();

                foreach (IFormFile file in product.ProductImagesFile)
                {
                    if (!file.CheckFileContentType("image/jpeg"))
                    {
                        ModelState.AddModelError("ProductImagesFile", "Secilen Seklin Novu Uygun");
                        return View(dbProduct);
                    }

                    if (!file.CheckFileSize(3000))
                    {
                        ModelState.AddModelError("ProductImagesFile", "Secilen Seklin Olcusu Maksimum 300 Kb Ola Biler");
                        return View(dbProduct);
                    }

                    ProductImage productImage = new ProductImage
                    {
                        Image = file.CreateFile(_env, "user", "assets", "img", "ProductImg"),
                        CreatedAt = DateTime.UtcNow.AddHours(4)
                    };

                    dbProduct.ProductImages.Add(productImage);
                }
            }
            if (product.DetailIds.Count > 0)
            {
                List<ProductDetail> productDetails = new List<ProductDetail>();
                _context.ProductDetails.RemoveRange(dbProduct.ProductDetails);

                if (dbProduct != null)
                {
                    foreach (var item in product.DetailIds)
                    {
                        DetailValue detailValue = await _context.DetailValues.FirstOrDefaultAsync(x => x.Id == item);

                        if (detailValue != null)
                        {
                            ProductDetail productDetail = new ProductDetail
                            {
                                DetailKeyId = detailValue.DetailKeyId,
                                DetailValueId = detailValue.Id,
                                ProductId = dbProduct.Id

                            };
                            productDetails.Add(productDetail);
                        }
                    }
                    await _context.ProductDetails.AddRangeAsync(productDetails);
                    await _context.SaveChangesAsync();
                }

            }



            dbProduct.BrandId = product.BrandId;
            dbProduct.CategoryId = product.CategoryId;
            dbProduct.Title = product.Title;
            dbProduct.Price = product.Price;
            dbProduct.DiscountPrice = product.DiscountPrice;
            dbProduct.Description = product.Description;
            dbProduct.Count = product.Count;
            dbProduct.Availability = product.Count > 0 ? true : false;
            dbProduct.UpdatedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return RedirectToAction("index", new { status, page });
        }
        public async Task<IActionResult> DeleteImage(int? id)
        {

            
            ViewBag.Categories = await _context.Categories.Where(c => !c.IsDeleted && !c.IsMain).ToListAsync();
            ViewBag.Tags = await _context.Tags.Where(t => !t.IsDeleted).ToListAsync();

            if (id == null) return BadRequest();


            Product product = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductTags).ThenInclude(pt => pt.Tag)
                .FirstOrDefaultAsync(p => p.ProductImages.Any(pi => pi.Id == id && !pi.IsDeleted));
            if (product.ProductImages.Count() == 1)
            {
                return View(product);
            }
            if (product == null) return NotFound();
            ViewBag.Brands = await _context.CategoryBrands
               .Include(x => x.Brand)
               .Include(x => x.Category)
               .Where(x => x.CategoryId == product.CategoryId).OrderByDescending(x => x.Id).ToListAsync();

            ViewBag.Details = await _context.CategoryDetailKeys
                .Include(x => x.DetailKey).ThenInclude(x => x.DetailValues)/*.ThenInclude(x => x.Brand)*/.Where(x => x.CategoryId == product.CategoryId)
                .Where(x => x.CategoryId == product.CategoryId).OrderBy(x => x.Id).ToListAsync();
            ViewBag.BrandId = product.BrandId;

            ViewBag.ProDetail = await _context.ProductDetails
                .Include(x => x.Product)
                .Include(x => x.DetailKey)
                .Include(x => x.DetailValue)
                .Where(x => x.ProductId == product.Id)
                .ToListAsync();


            Helper.DeleteFile(_env, product.ProductImages.FirstOrDefault(pi => pi.Id == id).Image, "user", "assets", "img", "ProductImg");

            _context.ProductImages.Remove(product.ProductImages.FirstOrDefault(pi => pi.Id == id));
            await _context.SaveChangesAsync();

            product.TagIds = product.ProductTags.Select(pt => pt.Tag.Id).ToList();

            return PartialView("_ProductUpdatePartial", product);
        }

        public IActionResult PageChange(string search, string key, string keyCtg, string keyBrand, string keyDetailKey, bool? status, int page = 1)
        {


            ViewBag.Status = status;

            IQueryable<Product> products = _context.Products
                .Include(x => x.ProductDetails).ThenInclude(x => x.DetailKey).ThenInclude(x=>x.DetailValues)
                .Include(x => x.Brand)
                .Include(x => x.Category).ThenInclude(x => x.CategoryDetailKeys)
                .Include(x => x.ProductTags).ThenInclude(x => x.Tag)
                .OrderByDescending(c => c.CreatedAt)
                .AsQueryable();

            if (search != null)
            {
                products = products.Where(c => c.Title.ToLower().Contains(search.ToLower())
                || c.Category.Name.ToLower().Contains(search.ToLower())
                || c.Brand.Name.ToLower().Contains(search.ToLower())
                );
            }
            else
            {
                if (status != null)
                    products = products.Where(c => c.IsDeleted == status);

                if (key != null)
                    products = products.Where(c => c.Title.ToLower().Contains(key.ToLower()));

                if (keyCtg != null)
                    products = products.Where(c => c.Category.Name.ToLower().Contains(keyCtg.ToLower()));

                if (keyBrand != null)
                    products = products.Where(c => c.Brand.Name.ToLower().Contains(keyBrand.ToLower()));

                
            }





            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)products.Count() / 5);
            return PartialView("_ProductIndexPartial", products.Skip((page - 1) * 5).Take(5));

        }

        public async Task<IActionResult> GetProMainDetail(int? id)
        {

            

            if (id == null) return BadRequest();

            Product product = await _context.Products
                .Include(p => p.ProductTags).ThenInclude(pt => pt.Tag)
                .Include(p => p.ProductDetails).ThenInclude(x => x.DetailKey).ThenInclude(x => x.DetailValues)
                .Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            ViewBag.Tags = await _context.Tags.Where(t => !t.IsDeleted).ToListAsync();
            product.TagIds = product.ProductTags.Select(pt => pt.Tag.Id).ToList();


            return PartialView("_GetProMainDetail",product);
        }





    }
}
