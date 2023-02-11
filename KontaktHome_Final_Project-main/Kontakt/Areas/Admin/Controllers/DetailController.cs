using Kontakt.DAL;
using Kontakt.Extensions;
using Kontakt.Helpers;
using Kontakt.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
    public class DetailController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public DetailController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(bool? status, int page = 1)
        {
            ViewBag.Status = status;

            IQueryable<DetailKey> detailKeys = _context.DetailKeys
                .Include(x=>x.CategoryDetailKeys).ThenInclude(x=>x.Category).ThenInclude(x=>x.CategoryBrands).ThenInclude(x=>x.Brand)
                .Include(x=>x.ProductDetails).ThenInclude(x=>x.Product)
                .OrderByDescending(c => c.CreatedAt)
                .AsQueryable();

            if (status != null)
                detailKeys = detailKeys.Where(c => c.IsDeleted == status);

            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)detailKeys.Count() / 8);
            return View(detailKeys.Skip((page - 1) * 8).Take(8).ToList());
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.SubCategory = await _context.Categories.Where(c => !c.IsMain && !c.IsDeleted).ToListAsync();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DetailKey detailKey, bool? status, int page = 1)
        {
            ViewBag.SubCategory = await _context.Categories.Where(c => !c.IsMain && !c.IsDeleted).ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (string.IsNullOrWhiteSpace(detailKey.Name))
            {
                ModelState.AddModelError("Name", "Bosluq Olmamalidir");
                return View();
            }

            List<CategoryDetailKey> NewcategoryDetailKeys = new List<CategoryDetailKey>();
            if (detailKey.CategoryIds.Count > 0)
            {
                
                List<CategoryDetailKey> categoryDetailKeys = new List<CategoryDetailKey>();

                foreach (int item in detailKey.CategoryIds)
                {
                    bool isAny = await _context.Categories.AnyAsync(t => t.Id == item && !t.IsMain);
                    if (!isAny)
                    {
                        ModelState.AddModelError("CategoryIds", $"Secilen Id {item} - li Category Yanlisdir");
                        return View();
                    }

                    if (await _context.DetailKeys.AnyAsync(t => t.Name.ToLower() == detailKey.Name.ToLower()))
                    {
                        
                        DetailKey DbdetailKey = _context.DetailKeys.Where(x => x.Name.ToLower() == detailKey.Name.ToLower()).FirstOrDefault();
                       
                        List<CategoryDetailKey> DbcategoryDetailKeys = _context.CategoryDetailKeys.Where(x => x.DetailKeyId == DbdetailKey.Id).ToList();
                        if (!await _context.CategoryDetailKeys.AnyAsync(x=>x.CategoryId==item))
                        {
                            if (!NewcategoryDetailKeys.Any(x => x.CategoryId == item))
                            {
                                CategoryDetailKey categoryDetailKey = new CategoryDetailKey
                                {
                                    CategoryId = item,
                                    DetailKeyId = DbdetailKey.Id
                                };
                                NewcategoryDetailKeys.Add(categoryDetailKey);
                            }
                        }
                        //foreach (CategoryDetailKey dbItem in DbcategoryDetailKeys)
                        //{
                        //    if (dbItem.CategoryId!=item)
                        //    {
                        //        if (!NewcategoryDetailKeys.Any(x=>x.CategoryId==item))
                        //        {
                        //            CategoryDetailKey categoryDetailKey = new CategoryDetailKey
                        //            {
                        //                CategoryId = item,
                        //                DetailKeyId = DbdetailKey.Id
                        //            };
                        //            NewcategoryDetailKeys.Add(categoryDetailKey);
                        //        }

                               

                                
                        //    }
                        //}


                    }
                    else
                    {
                        CategoryDetailKey categoryDetailKey = new CategoryDetailKey
                        {
                            CategoryId = item
                        };

                        categoryDetailKeys.Add(categoryDetailKey);
                    }

                    
                }

                detailKey.CategoryDetailKeys = categoryDetailKeys;
            }
            else
            {
                    ModelState.AddModelError("CategoryIds", "Category secilmeyib");
                    return View();
            }
            if (detailKey.CategoryDetailKeys.Count()>0)
            {
                detailKey.CreatedAt = DateTime.UtcNow.AddHours(4);
                await _context.DetailKeys.AddAsync(detailKey);
                await _context.SaveChangesAsync();
            }
            else if (NewcategoryDetailKeys.Count()>0)
            {
                await _context.CategoryDetailKeys.AddRangeAsync(NewcategoryDetailKeys);
                await _context.SaveChangesAsync();
            }
            else
            {
                ModelState.AddModelError("Name", "Alreade Exists");
                return View();
            }




            return RedirectToAction("Index", new { status = status, page = page });
        }

        public async Task<IActionResult> Update(int? id, bool? status, int page = 1)
        {
            ViewBag.Status = status;
            ViewBag.PageIndex = page;
            ViewBag.SubCategory = await _context.Categories.Where(c => !c.IsMain && !c.IsDeleted).ToListAsync();
            if (id == null) return BadRequest();

            DetailKey detailKey = await _context.DetailKeys
                .Include(c=>c.CategoryDetailKeys)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (detailKey == null) return NotFound();

           
            detailKey.CategoryIds = detailKey.CategoryDetailKeys.Select(pt => pt.Category.Id).ToList();

            return View(detailKey);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, DetailKey detailKey, bool? status, bool? isMainRoute, int page = 1)
        {
            ViewBag.SubCategory = await _context.Categories.Where(c => !c.IsMain && !c.IsDeleted).ToListAsync();


            DetailKey DbdetailKey = await _context.DetailKeys
               .Include(c => c.CategoryDetailKeys).ThenInclude(c=>c.Category)
               .FirstOrDefaultAsync(c => c.Id == id);

            if (DbdetailKey == null) return NotFound();

            


            if (!ModelState.IsValid && detailKey.CategoryIds==null)
            {
                return View(DbdetailKey);
            }

            if (id != detailKey.Id) return BadRequest();

            if (string.IsNullOrWhiteSpace(detailKey.Name))
            {
                ModelState.AddModelError("Name", "Bosluq Olmamalidir");
                return View(DbdetailKey);
            }


            if (await _context.DetailKeys.AnyAsync(t => t.Id != id && t.Name.ToLower() == detailKey.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "Alreade Exists");
                return View(DbdetailKey);
            }
            if (detailKey.CategoryIds.Count > 0)
            {
                
                _context.CategoryDetailKeys.RemoveRange(DbdetailKey.CategoryDetailKeys);

                List<CategoryDetailKey> categoryDetailKeys = new List<CategoryDetailKey>();

                foreach (int item in detailKey.CategoryIds)
                {
                    bool isAny = await _context.Categories.AnyAsync(t => t.Id == item && !t.IsMain);
                    if (!isAny)
                    {
                        ModelState.AddModelError("CategoryIds", $"Secilen Id {item} - li Category Yanlisdir");
                        return View();
                    }

                    CategoryDetailKey categoryDetailKey = new CategoryDetailKey
                    {
                        CategoryId = item
                    };

                    categoryDetailKeys.Add(categoryDetailKey);
                }

                DbdetailKey.CategoryDetailKeys = categoryDetailKeys;


            }
            else
            {
                _context.CategoryDetailKeys.RemoveRange(DbdetailKey.CategoryDetailKeys);
            }




            DbdetailKey.Name = detailKey.Name;
            DbdetailKey.isMain = detailKey.isMain;
            DbdetailKey.ForTitle = detailKey.ForTitle;
            DbdetailKey.UpdatedAt = DateTime.UtcNow.AddHours(4);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { status = status, isMainRoute = isMainRoute, page = page });
        }
        public async Task<IActionResult> DeleteRestore(int? id, string key,  bool? status, int page = 1)
        {
            if (id == null) return BadRequest();

            DetailKey DbdetailKey = await _context.DetailKeys.FirstOrDefaultAsync(t => t.Id == id);

            if (DbdetailKey == null) return NotFound();

            if (DbdetailKey.IsDeleted)
            {
                DbdetailKey.IsDeleted = false;
            }
            else
            {
                DbdetailKey.IsDeleted = true;
                DbdetailKey.DeletedAt = DateTime.UtcNow.AddHours(4);
            }


            await _context.SaveChangesAsync();

            ViewBag.Status = status;

            IQueryable<DetailKey> detailKeys = _context.DetailKeys
                .Include(x => x.CategoryDetailKeys).ThenInclude(x => x.Category)
                .Include(x => x.ProductDetails).ThenInclude(x => x.Product)
                .OrderByDescending(c => c.CreatedAt)
                .AsQueryable();

            if (status != null)
                detailKeys = detailKeys.Where(c => c.IsDeleted == status);

            if (key != null)
            {
                detailKeys = detailKeys.Where(c => c.Name.ToLower().Contains(key.ToLower()));
            }

            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)detailKeys.Count() / 8);
            return PartialView("_DetailindexPartial", detailKeys.Skip((page - 1) * 8).Take(8));

        }

        public IActionResult PageChange(string key, bool? status, int page = 1)
        {
            IQueryable<DetailKey> detailKeys = _context.DetailKeys
                 .Include(x => x.CategoryDetailKeys).ThenInclude(x => x.Category)
                 .Include(x => x.ProductDetails).ThenInclude(x => x.Product)
                 .OrderByDescending(c => c.CreatedAt)
                 .AsQueryable();

            if (status != null)
            {
                detailKeys = detailKeys.Where(c => c.IsDeleted == status);
            }

            if (key != null)
            {
                detailKeys = detailKeys.Where(c => c.Name.ToLower().Contains(key.ToLower()));
            }


            ViewBag.Status = status;
            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)detailKeys.Count() / 8);
            return PartialView("_DetailindexPartial", detailKeys.Skip((page - 1) * 8).Take(8));

        }

        public IActionResult ValueIndex(bool? status, int page = 1)
        {
            ViewBag.Status = status;

            IQueryable<DetailValue> detailValues = _context.DetailValues
                .Include(x => x.ProductDetails).ThenInclude(x => x.Product)
                .Include(x=>x.DetailKey).ThenInclude(x=>x.CategoryDetailKeys).ThenInclude(x=>x.Category)

                .Include(x=>x.Category)
                .OrderByDescending(c => c.CreatedAt)
                .AsQueryable();

            if (status != null)
                detailValues = detailValues.Where(c => c.IsDeleted == status);

            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)detailValues.Count() / 8);
            return View(detailValues.Skip((page - 1) * 8).Take(8).ToList());
        }
        public async Task<IActionResult> ValueCreate()
        {
            ViewBag.DetailKey = await _context.DetailKeys.Where(c =>!c.IsDeleted).ToListAsync();
            ViewBag.MainCategory = await _context.Categories.Where(c => c.IsMain && !c.IsDeleted).ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ValueCreate(DetailValue detailValue, bool? status, int page = 1)
        {
            ViewBag.DetailKey = await _context.DetailKeys.Where(c => !c.IsDeleted).ToListAsync();
            ViewBag.MainCategory = await _context.Categories.Where(c => c.IsMain && !c.IsDeleted).ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }

            
            if (detailValue.CategoryId==null)
            {
                ModelState.AddModelError(" ", "Category Empty");
                return View();
            }
            if (detailValue.DetailKeyId==null)
            {
                ModelState.AddModelError(" ", "Detail Key Empty");
                return View();
            }
            //if (detailValue.BrandId == null)
            //{
            //    ModelState.AddModelError(" ", "Brand Empty");
            //    return View();
            //}

            bool isAny = await _context.Categories.AnyAsync(t => t.Id == detailValue.CategoryId && !t.IsMain);
            if (!isAny)
            {
                ModelState.AddModelError(" ", $"Secilen Category Yanlisdir");
                return View();
            }
            bool isAnyKey = await _context.DetailKeys.AnyAsync(t => t.Id == detailValue.DetailKeyId);
            if (!isAnyKey)
            {
                ModelState.AddModelError(" ", $"Secilen Detail Yanlisdir");
                return View();
            }
            //bool isAnyBrand = await _context.Brands.AnyAsync(t => t.Id == detailValue.BrandId);
            //if (!isAnyKey)
            //{
            //    ModelState.AddModelError(" ", $"Secilen Brand Yanlisdir");
            //    return View();
            //}

            List<DetailValue> detailValues = new List<DetailValue>();
            if (detailValue.Names.Count > 0)
            {
                

                foreach (string item in detailValue.Names)
                {

                    if (string.IsNullOrWhiteSpace(item))
                    {
                        ModelState.AddModelError(" ", "Value is Empty");
                        return View();
                    }
                    else
                    if (!await _context.DetailValues.AnyAsync(t => t.Name.ToLower() == item.ToLower() && t.DetailKeyId==detailValue.DetailKeyId && t.CategoryId==detailValue.CategoryId/* && detailValue.BrandId==t.BrandId */))
                    {
                        DetailValue detailValue1 = new DetailValue
                        {
                            Name = item,
                            CategoryId = detailValue.CategoryId,
                            //BrandId = detailValue.BrandId,
                            DetailKeyId = detailValue.DetailKeyId,
                            CreatedAt = DateTime.UtcNow.AddHours(4)
                        };

                        detailValues.Add(detailValue1);
                    }
                    
                    

                    
                }

                
            }

            //foreach (var item in detailValues)
            //{
            //    DetailValue detailValue1 = new DetailValue
            //    {
            //        Name = item.Name,
            //        CategoryId = item.CategoryId,
            //        BrandId = item.BrandId,
            //        DetailKeyId = item.DetailKeyId,
            //        CreatedAt = DateTime.UtcNow.AddHours(4)

            //    };


            //    await _context.DetailValues.AddAsync(detailValue1);
            //    await _context.SaveChangesAsync();
            //}

            await _context.DetailValues.AddRangeAsync(detailValues);
            await _context.SaveChangesAsync();
            return RedirectToAction("ValueIndex", new { status = status, page = page });
        }
        public async Task<IActionResult> GetDetailKeyList(int? id)
        {

            List<CategoryDetailKey> categoryDetailKeys = await _context.CategoryDetailKeys
                .Include(x=>x.DetailKey).ThenInclude(x=>x.DetailValues)
                .Include(x=>x.Category)
                .Where(x => x.CategoryId == id).OrderByDescending(x => x.Id).ToListAsync();

            return PartialView("_GetDetailKeyPartial", categoryDetailKeys);

        }
        public async Task<IActionResult> UpdateValue(int? id, bool? status, int page = 1)
        {
            ViewBag.Status = status;
            ViewBag.PageIndex = page;
            ViewBag.SubCategory = await _context.Categories.Where(c => !c.IsMain && !c.IsDeleted).ToListAsync();
            if (id == null) return BadRequest();

            DetailValue detailValue = await _context.DetailValues
                .FirstOrDefaultAsync(c => c.Id == id);
            ViewBag.Brands = await _context.CategoryBrands
                .Include(x=>x.Brand)
                .Where(b => b.CategoryId == detailValue.CategoryId)
                .ToListAsync();

            ViewBag.DetailKeys = await _context.CategoryDetailKeys
                .Include(x=>x.Category)
                .Include(x=>x.DetailKey).ThenInclude(x=>x.DetailValues)
               .Where(b => b.CategoryId == detailValue.CategoryId)
               .ToListAsync();
            if (detailValue == null) return NotFound();


            return View(detailValue);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateValue(int? id, DetailValue detailValue, bool? status, bool? isMainRoute, int page = 1)
        {

            ViewBag.Status = status;
            ViewBag.PageIndex = page;
            ViewBag.SubCategory = await _context.Categories.Where(c => !c.IsMain && !c.IsDeleted).ToListAsync();
            
            DetailValue DbdetailValue = await _context.DetailValues
                .FirstOrDefaultAsync(c => c.Id == id);

            if (DbdetailValue == null) return NotFound();
            ViewBag.Brands = await _context.CategoryBrands
                .Include(x => x.Brand)
                .Where(b => b.CategoryId == DbdetailValue.CategoryId)
                .ToListAsync();

            ViewBag.DetailKeys = await _context.CategoryDetailKeys
                .Include(x => x.Category)
                .Include(x => x.DetailKey).ThenInclude(x => x.DetailValues)
               .Where(b => b.CategoryId == DbdetailValue.CategoryId)
               .ToListAsync();

            if (id != detailValue.Id) return BadRequest();

            if (string.IsNullOrWhiteSpace(detailValue.Name))
            {
                ModelState.AddModelError("Name", "Bosluq Olmamalidir");
                return View(DbdetailValue);
            }


            if (await _context.DetailKeys.AnyAsync(t => t.Id != id && t.Name.ToLower() == detailValue.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "Alreade Exists");
                return View(DbdetailValue);
            }
            bool isAny = await _context.Categories.AnyAsync(t => t.Id == detailValue.CategoryId && !t.IsMain);
            if (!isAny)
            {
                ModelState.AddModelError("", $"Secilen  {detailValue.CategoryId}id - li Category Yanlisdir");
                return View(DbdetailValue);
            }
            //bool isAnyBrand = await _context.Brands.AnyAsync(t => t.Id == detailValue.BrandId);
            //if (!isAnyBrand)
            //{
            //    ModelState.AddModelError("", $"Secilen  {detailValue.BrandId} - li Brand Yanlisdir");
            //    return View(DbdetailValue);
            //}
            bool isAnyDetailKey = await _context.DetailKeys.AnyAsync(t => t.Id == detailValue.DetailKeyId);
            if (!isAnyDetailKey)
            {
                ModelState.AddModelError("", $"Secilen  {detailValue.DetailKeyId} id - li DetailKey Yanlisdir");
                return View(DbdetailValue);
            }
            DbdetailValue.Name = detailValue.Name;
            DbdetailValue.CategoryId = detailValue.CategoryId;
            //DbdetailValue.BrandId = detailValue.BrandId;
            DbdetailValue.DetailKeyId = detailValue.DetailKeyId;
            DbdetailValue.UpdatedAt = DateTime.UtcNow.AddHours(4);
            await _context.SaveChangesAsync();

            return RedirectToAction("ValueIndex", new { status = status, isMainRoute = isMainRoute, page = page });
        }
        public IActionResult PageChangeValue(string search, string key, string keyCtg, string keyBrand, string keyDetailKey, bool? status, int page = 1)
        {


            ViewBag.Status = status;

            IQueryable<DetailValue> detailValues = _context.DetailValues
                .Include(x => x.ProductDetails).ThenInclude(x => x.Product)
                .Include(x => x.DetailKey).ThenInclude(x => x.CategoryDetailKeys).ThenInclude(x => x.Category)
                //.Include(x => x.Brand)
                .Include(x => x.Category)
                .OrderByDescending(c => c.CreatedAt)
                .AsQueryable();
            if(search!=null)
            {
                detailValues = detailValues.Where(c => c.Name.ToLower().Contains(search.ToLower())
                || c.Category.Name.ToLower().Contains(search.ToLower())
                //|| c.Brand.Name.ToLower().Contains(search.ToLower())
                || c.DetailKey.Name.ToLower().Contains(search.ToLower())
                );
            }
            else
            {
                if (status != null)
                    detailValues = detailValues.Where(c => c.IsDeleted == status);

                if (key != null)
                    detailValues = detailValues.Where(c => c.Name.ToLower().Contains(key.ToLower()));

                if (keyCtg != null)
                    detailValues = detailValues.Where(c => c.Category.Name.ToLower().Contains(keyCtg.ToLower()));

                //if (keyBrand != null)
                //    detailValues = detailValues.Where(c => c.Brand.Name.ToLower().Contains(keyBrand.ToLower()));

                if (keyDetailKey != null)
                    detailValues = detailValues.Where(c => c.DetailKey.Name.ToLower().Contains(keyDetailKey.ToLower()));
            }
           



            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)detailValues.Count() / 8);
            return PartialView("_DetailValueIndex", detailValues.Skip((page - 1) * 8).Take(8));

        }
        public async Task<IActionResult> DeleteRestoreValue(int? id, string key, string keyCtg, string keyBrand, string keyDetailKey, bool? status, int page = 1)
        {
            if (id == null) return BadRequest();

            DetailValue DbdetailValue = await _context.DetailValues.FirstOrDefaultAsync(t => t.Id == id);

            if (DbdetailValue == null) return NotFound();

            if (DbdetailValue.IsDeleted)
            {
                DbdetailValue.IsDeleted = false;
            }
            else
            {
                DbdetailValue.IsDeleted = true;
                DbdetailValue.DeletedAt = DateTime.UtcNow.AddHours(4);
            }


            await _context.SaveChangesAsync();

            ViewBag.Status = status;

            IQueryable<DetailValue> detailValues = _context.DetailValues
                .Include(x => x.ProductDetails).ThenInclude(x => x.Product)
                .Include(x => x.DetailKey).ThenInclude(x => x.CategoryDetailKeys).ThenInclude(x => x.Category)
                //.Include(x => x.Brand)
                .Include(x => x.Category)
                .OrderByDescending(c => c.CreatedAt)
                .AsQueryable();

            if (status != null)
                detailValues = detailValues.Where(c => c.IsDeleted == status);

            if (key != null)
                detailValues = detailValues.Where(c => c.Name.ToLower().Contains(key.ToLower()));

            if (keyCtg != null)
                detailValues = detailValues.Where(c => c.Category.Name.ToLower().Contains(keyCtg.ToLower()));

            //if (keyBrand != null)
            //    detailValues = detailValues.Where(c => c.Brand.Name.ToLower().Contains(keyBrand.ToLower()));

            if (keyDetailKey != null)
                detailValues = detailValues.Where(c => c.DetailKey.Name.ToLower().Contains(keyDetailKey.ToLower()));

            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)detailValues.Count() / 8);
            return PartialView("_DetailValueIndex", detailValues.Skip((page - 1) * 8).Take(8));

        }
    }
}
