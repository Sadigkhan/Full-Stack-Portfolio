using Kontakt.DAL;
using Kontakt.Models;
using Kontakt.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontakt.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        public ProductController
            (
                UserManager<AppUser> userManager,
                SignInManager<AppUser> signInManager,
                AppDbContext context,
                RoleManager<IdentityRole> roleManager,
                IWebHostEnvironment env,
                IConfiguration config
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _roleManager = roleManager;
            _env = env;
            _config = config;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetProducts(int? id, string keyfrom)
        {
            string cookieWish = HttpContext.Request.Cookies["Wish"];

            List<WishVM> wishVMs = null;

            if (cookieWish != null)
            {
                wishVMs = JsonConvert.DeserializeObject<List<WishVM>>(cookieWish);
            }
            else
            {
                wishVMs = new List<WishVM>();
            }

            ViewBag.WishList = wishVMs;


            if (id == null)
            {
                List<Product> products = await _context.Products
                    .Include(x=>x.ProductDetails).ThenInclude(x=>x.DetailKey).ThenInclude(x=>x.DetailValues)
                .Where(x => !x.IsDeleted).OrderBy(x => x.CreatedAt).Take(4).ToListAsync();
                if (keyfrom == "home")
                {
                    return PartialView("_GetProductsPartial", products);
                }
                else
                {
                    return View(products);
                }
            }
            else
            {
                List<Product> products = await _context.Products
                .Include(x => x.ProductDetails).ThenInclude(x => x.DetailKey).ThenInclude(x => x.DetailValues)
                .Where(x => !x.IsDeleted && x.CategoryId == id).OrderBy(x => x.CreatedAt).Take(4).ToListAsync();
                if (products == null) return BadRequest();
                
                return PartialView("_GetProductsPartial", products);
                
                
            }





        }

        public async Task<IActionResult> LoadMoreProducts(int skip,int ctg, int? brand, string key)
        {
            if (key=="smilar")
            {
                List<Product> products = await _context.Products
                    .Include(p => p.ProductImages)
                    .Include(x => x.Reviews)
                    .Include(x => x.Category)
                    .Include(x => x.ProductDetails).ThenInclude(x => x.DetailKey).ThenInclude(x => x.DetailValues)
                    .Where(p => !p.IsDeleted && p.CategoryId==ctg && p.BrandId==brand).Skip(skip).Take(4).OrderBy(x => x.CreatedAt).ToListAsync();
                return PartialView("_GetProductsPartial", products);
            }
            else if (key == "smilarCtg")
            {
                List<Product> products = await _context.Products
                    .Include(p => p.ProductImages)
                    .Include(x => x.Reviews)
                    .Include(x => x.Category)
                    .Include(x => x.ProductDetails).ThenInclude(x => x.DetailKey).ThenInclude(x => x.DetailValues)
                    .Where(p => !p.IsDeleted && p.CategoryId == ctg).Skip(skip).Take(4).OrderByDescending(x => x.Price).ToListAsync();
                return PartialView("_GetProductsPartial", products);
            }
            else
            {
                List<Product> Disproducts = new List<Product>();
                List<Product> proList = new List<Product>();

                List<Product> products = await _context.Products
                    .Include(p => p.ProductImages)
                    .Include(x => x.Reviews)
                    .Include(x => x.Category)
                    .Include(x => x.ProductDetails).ThenInclude(x => x.DetailKey).ThenInclude(x => x.DetailValues)
                    .Where(p => !p.IsDeleted && p.DiscountPrice > 0).OrderBy(x => x.CreatedAt).ToListAsync();

                foreach (Product product in products)
                {

                    if (proList.Where(x => x.CategoryId == product.CategoryId).Count() == 0)
                    {
                        if ((100 - ((product.DiscountPrice / product.Price) * 100) > 0))
                        {
                            proList.Add(product);
                        }

                    }
                    else
                    {
                        if ((100 - ((product.DiscountPrice / product.Price) * 100) > (100 - ((proList.LastOrDefault(p => p.CategoryId == product.CategoryId).DiscountPrice / proList.LastOrDefault(p => p.CategoryId == product.CategoryId).Price) * 100))))
                        {
                            proList.Remove(proList.LastOrDefault(p => p.CategoryId == product.CategoryId));
                            proList.Add(product);
                        }
                    }

                }
                Disproducts.AddRange(proList.OrderByDescending(x => 100 - (x.DiscountPrice / x.Price) * 100).Skip(skip).Take(4));



                return PartialView("_GetProductsPartial", Disproducts);
            }

            
            

        }

        public async Task<IActionResult> ProductDetail(int? id)
        {
            if (id == null) return BadRequest();
            Product product = await _context.Products
                .Include(x => x.Brand).ThenInclude(x=>x.CategoryBrands)
                .Include(x => x.ProductImages)
                .Include(x => x.Category).ThenInclude(x=>x.Parent).ThenInclude(x=>x.Children)
                .Include(x => x.Reviews)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (product == null) return NotFound();
            List<Product> products = await _context.Products
                .Include(x => x.ProductDetails).ThenInclude(x => x.DetailKey).ThenInclude(x => x.DetailValues)
                .Where(x => x.CategoryId == product.CategoryId && x.BrandId==product.BrandId).OrderBy(x => x.CreatedAt).ToListAsync();
            List<Product> productsvm = products.Take(4).ToList();
            ViewBag.proCount = products.Count();
            ProductDetailVM ProductDetailVM = new ProductDetailVM
            {
                Product = product,
                Products = productsvm,
                CategoryBrand = await _context.CategoryBrands.Where(x => x.BrandId == product.BrandId).FirstOrDefaultAsync()
                
            };

            return View(ProductDetailVM);
        }
        public async Task<IActionResult> GetProDetail(int? id,string key)
        {
            if (id == null) return BadRequest();
            Product product = await _context.Products
                .Include(x=>x.ProductDetails).ThenInclude(x=>x.DetailKey).ThenInclude(x=>x.DetailValues)
                    .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (product == null) return NotFound();

            ViewBag.Title = product.Title ;


            if (key=="detail")
            {
                List<ProductDetail> productDetails = await _context.ProductDetails
                .Include(x => x.DetailKey).Include(x => x.DetailValue)
                .Include(x => x.Product).Where(x => x.ProductId == product.Id).ToListAsync();
                
                return PartialView("_GetProDetailPartial", productDetails);
            }
            else if (key=="desc")
            {
                return PartialView("_GetProDescPartial", product);
            }
            else if (key=="rew")
            {
                List<Review> reviews = await _context.Reviews
                .Include(x => x.Product)
                .Include(x=>x.AppUser)
                .Where(x => x.ProductId == product.Id).ToListAsync();
                var TitleAdd = string.Join(" ", product.ProductDetails.Where(x => x.DetailKey.ForTitle).OrderByDescending(x => x.DetailKey.UpdatedAt).Select(p => p.DetailValue.Name));
                ViewBag.Title = product.Title + " " + TitleAdd;
                
                return PartialView("_GetProRewPartial", reviews);
            }
            else
            {
                return BadRequest();
            }


            
        }

        public async Task<IActionResult> ProductList(int? id, string keyfrom)
        {

            HttpContext.Response.Cookies.Delete("filter");


            if (keyfrom=="ctg")
            {
                List<Product> products = await _context.Products
                .Include(x => x.Category).ThenInclude(x => x.Parent)
                .Include(x=>x.ProductDetails).ThenInclude(x=>x.DetailKey).ThenInclude(x=>x.DetailValues).OrderBy(x=>x.UpdatedAt)
               .Where(x => !x.IsDeleted && x.CategoryId == id).OrderByDescending(x => x.Price).ToListAsync();

                List<Product> productsLast = new List<Product>();
                productsLast.AddRange(products.Take(6));
                ViewBag.procount = products.Count();

                Category category = await _context.Categories
                    .Include(x=>x.Children)
                    .Include(x=>x.Parent).ThenInclude(x=>x.Parent)
                    .Where(x => x.Id == id).FirstOrDefaultAsync();
                if (category==null || category.Children.Count()>0)
                {
                    return NotFound();
                }
                List<CategoryDetailKey> categoryDetailKeys = await _context.CategoryDetailKeys
                    .Include(x=>x.DetailKey).ThenInclude(x=>x.DetailValues)
                    .Where(x => x.CategoryId == id && x.DetailKey.isMain).OrderByDescending(x=>x.Id).ToListAsync();
                List<DetailValue> detailValues = new List<DetailValue>();

                foreach (Product product in products)
                {
                    foreach (ProductDetail productDetail in product.ProductDetails)
                    {
                        detailValues.Add(productDetail.DetailValue);
                    }
                }


                ViewBag.ProductsDetail = detailValues;
                ProductListVM productListVM = new ProductListVM
                {
                    Products = productsLast,
                    Category = category,
                    CategoryDetailKeys = categoryDetailKeys
                    
                };

                ViewBag.keyfrom = keyfrom;
                return View(productListVM);
            }
            else if (keyfrom == "brand")
            {
                CategoryBrand categoryBrand = await _context.CategoryBrands
                    .Where(x => x.Id == id).FirstOrDefaultAsync();
                if (categoryBrand == null)
                {
                    return NotFound();
                }
                Category category = await _context.Categories
                    .Include(x => x.Parent)
                    .Where(x => x.Id == categoryBrand.CategoryId).FirstOrDefaultAsync();
                Brand brand = await _context.Brands
                    .Where(x => x.Id == categoryBrand.BrandId).FirstOrDefaultAsync();

                List<CategoryDetailKey> categoryDetailKeys = await _context.CategoryDetailKeys
                    .Include(x => x.DetailKey).ThenInclude(x => x.DetailValues)
                    .Where(x => x.CategoryId == category.Id && x.DetailKey.isMain).ToListAsync();


                List<Product> products = await _context.Products
                    .Include(x => x.ProductDetails).ThenInclude(x => x.DetailKey).ThenInclude(x => x.DetailValues).OrderBy(x => x.UpdatedAt)
                .Include(x => x.Category).ThenInclude(x => x.Parent)
               .Where(x => !x.IsDeleted && x.CategoryId == category.Id && x.BrandId == brand.Id).OrderByDescending(x => x.Price).ToListAsync();

                List<Product> productsLast = new List<Product>();
                productsLast.AddRange(products.Take(6));
                ViewBag.procount = products.Count();

                List<DetailValue> detailValues = new List<DetailValue>();

                foreach (Product product in products)
                {
                    foreach (ProductDetail productDetail in product.ProductDetails)
                    {
                        detailValues.Add(productDetail.DetailValue);
                    }
                }


                ViewBag.ProductsDetail = detailValues;


                ProductListVM productListVM = new ProductListVM
                {
                    Products = productsLast,
                    Category = category,
                    Brand = brand,
                    CategoryBrand = categoryBrand,
                    CategoryDetailKeys = categoryDetailKeys

                };

                ViewBag.keyfrom = keyfrom;
                return View(productListVM);
            }
            else
            {
                return NotFound();
            }
            
            

           





        }

        public async Task<IActionResult>  FilterProList(int? keyDetailId, int? keyValId,int? brandId, int? ctgId,int? skip=0)
        {

           

            string cookieFilter = HttpContext.Request.Cookies["filter"];
            List<FilterVM> filterVMs = null;
            if (keyDetailId != null && keyValId != null)
            {
                

                if (cookieFilter != null)
                {
                    filterVMs = JsonConvert.DeserializeObject<List<FilterVM>>(cookieFilter);

                    if (filterVMs.Any(f => f.DetailId == keyDetailId && f.DetailVal == keyValId))
                    {
                        FilterVM filterVM = filterVMs.FirstOrDefault(b => b.DetailId == keyDetailId && b.DetailVal == keyValId);
                        if (filterVM == null) return NotFound();


                        filterVMs.Remove(filterVM);
                    }
                    else
                    {

                        filterVMs.Add(new FilterVM
                        {
                            DetailId = (int)keyDetailId,
                            DetailVal = (int)keyValId

                        });
                    }
                }
                else
                {

                    filterVMs = new List<FilterVM>();
                    filterVMs.Add(new FilterVM
                    {
                        DetailId = (int)keyDetailId,
                        DetailVal = (int)keyValId

                    });
                }

                cookieFilter = JsonConvert.SerializeObject(filterVMs);
                HttpContext.Response.Cookies.Append("filter", cookieFilter);
            }


            if (cookieFilter != null)
            {
                filterVMs = JsonConvert.DeserializeObject<List<FilterVM>>(cookieFilter);
            }
            else
            {
                filterVMs = new List<FilterVM>();
            }

            List<FilterVM> filters = filterVMs;

            List<Product> productsFildered = new List<Product>();
            List<Product> test = new List<Product>();
            List<Product> last = new List<Product>();
            
            if (filters.Count==0)
            {
                if (brandId!=null)
                {
                    CategoryBrand categoryBrand = await _context.CategoryBrands
                    .Where(x => x.Id == brandId).FirstOrDefaultAsync();

                    Brand brand = await _context.Brands
                    .Where(x => x.Id == categoryBrand.BrandId).FirstOrDefaultAsync();

                    List<Product> products = await _context.Products
                    .Include(x => x.ProductDetails).ThenInclude(x => x.DetailKey).ThenInclude(x => x.DetailValues)
                    .Include(x => x.Category).ThenInclude(x => x.Parent)
                   .Where(x => !x.IsDeleted && x.BrandId==brand.Id && x.CategoryId==categoryBrand.CategoryId).OrderByDescending(x => x.Price).ToListAsync();
                    productsFildered.AddRange(products);
                }
                else if (ctgId!=null)
                {
                    List<Product> products = await _context.Products
                    .Include(x => x.ProductDetails).ThenInclude(x => x.DetailKey).ThenInclude(x => x.DetailValues)
                    .Include(x => x.Category).ThenInclude(x => x.Parent)
                   .Where(x => !x.IsDeleted && x.CategoryId == ctgId).OrderByDescending(x => x.Price).ToListAsync();
                    productsFildered.AddRange(products);
                }
                

                

                
            }
            else
            {




                int first = filters.FirstOrDefault().DetailId;
                foreach (FilterVM item in filters)
                {
                    //productsFildered = products.Where(p => p.ProductDetails.Any(pd => pd.DetailKeyId == item.DetailId && pd.DetailValueId == item.DetailVal));

                    if (item.DetailId==first)
                    {
                        if (ctgId!=null)
                        {
                            productsFildered.AddRange(await _context.Products.Where(x => x.CategoryId == ctgId).Include(pd => pd.ProductDetails).ThenInclude(x => x.DetailKey).ThenInclude(x => x.DetailValues).Where(p => p.ProductDetails.Any(pd => pd.DetailKeyId == item.DetailId && pd.DetailValueId == item.DetailVal)).ToListAsync());

                        }
                        else
                        {
                            CategoryBrand categoryBrand = await _context.CategoryBrands
                            .Where(x => x.Id == brandId).FirstOrDefaultAsync();

                            Brand brand = await _context.Brands
                            .Where(x => x.Id == categoryBrand.BrandId).FirstOrDefaultAsync();
                            productsFildered.AddRange(await _context.Products.Where(x => x.BrandId == brand.Id).Include(pd => pd.ProductDetails).ThenInclude(x => x.DetailKey).ThenInclude(x => x.DetailValues).Where(p => p.ProductDetails.Any(pd => pd.DetailKeyId == item.DetailId && pd.DetailValueId == item.DetailVal)).ToListAsync());

                        }
                    }
                    else
                    {
                        foreach (var product in productsFildered)
                        {
                            if (!product.ProductDetails.Any(pd => pd.DetailKeyId == item.DetailId && pd.DetailValueId == item.DetailVal))
                            {
                                if (!test.Any(x=>x.Id==product.Id))
                                {
                                    test.Add(product);
                                }
                               
                            }
                        }

                        
                    }

                   
                   
                }
                last.AddRange(productsFildered);
                foreach (var item3 in last)
                {
                    if (test.Any(pd=>pd.Id==item3.Id))
                    {
                        productsFildered.Remove(item3);
                    }
                }
                
                

            }
            ViewBag.procount = productsFildered.Count();

            return PartialView("_GetFilterProList", productsFildered.Skip((int)skip).Take(6));


        }

        public async Task<IActionResult> AddBasket(int? id)
        {
            if (id == null) return BadRequest();

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            string cookieBasket = HttpContext.Request.Cookies["basket"];

            List<BasketVM> basketVMs = null;

            if (product.Count==0)
            {
                return Content("no");
            }

            if (cookieBasket != null)
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(cookieBasket);

                if (basketVMs.Any(b => b.ProductId == id))
                {
                    if (basketVMs.Find(b => b.ProductId == id).Count<product.Count)
                    {
                        basketVMs.Find(b => b.ProductId == id).Count += 1;
                    }
                    
                }
                else
                {

                    basketVMs.Add(new BasketVM
                    {

                        ProductId = (int)id,
                        Count = 1,
                    });
                }
            }
            else
            {
                basketVMs = new List<BasketVM>();
                basketVMs.Add(new BasketVM
                {
                    ProductId = (int)id,
                    Count = 1,
                });
            }

            cookieBasket = JsonConvert.SerializeObject(basketVMs);
            HttpContext.Response.Cookies.Append("basket", cookieBasket);

            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name && !u.isAdmin);

            if (appUser!=null)
            {
                
                List<Basket> existedBasket = await _context.Baskets.Where(b => b.AppUserId == appUser.Id).ToListAsync();

                if (existedBasket.Any(b => b.ProductId == id))
                {
                    foreach (Basket basket in existedBasket)
                    {
                        if (basket.ProductId==id && basket.Count<product.Count)
                        {
                            basket.Count += 1;
                            basket.UpdatedAt = DateTime.UtcNow.AddHours(4);
                        }
                    }

                }
                else
                {

                    Basket basket = new Basket { 
                        AppUserId=appUser.Id,
                        Count=1,
                        ProductId=id,
                        CreatedAt= DateTime.UtcNow.AddHours(4),

                    };


                    _context.Baskets.Add(basket);
                    
                }
                await _context.SaveChangesAsync();
            }



            return Content("ok");



        }

        public async Task<IActionResult> AddLikelist(int? id)
        {
            if (id == null) return BadRequest();

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            

            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name && !u.isAdmin);

            if (appUser != null)
            {
                Like like = await _context.Likes.Where(x => x.AppUserId == appUser.Id && x.ProductId == product.Id).FirstOrDefaultAsync();
                DisLike disLike= await _context.DisLikes.Where(x => x.AppUserId == appUser.Id && x.ProductId == product.Id).FirstOrDefaultAsync();
                if (like==null)
                {
                    Like Newlike = new Like()
                    {
                        ProductId = product.Id,
                        AppUserId=appUser.Id
                        
                    };
                    _context.Likes.Add(Newlike);
                   await _context.SaveChangesAsync();
                    
                }
                else
                {
                    _context.Likes.Remove(like);
                    await _context.SaveChangesAsync();
                }

                if (disLike!=null)
                {
                    _context.DisLikes.Remove(disLike);
                    await _context.SaveChangesAsync();
                }

            }



            return Content("ok");



        }
        public async Task<IActionResult> AddDisLikelist(int? id)
        {
            if (id == null) return BadRequest();

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();



            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name && !u.isAdmin);

            if (appUser != null)
            {
                Like like = await _context.Likes.Where(x => x.AppUserId == appUser.Id && x.ProductId == product.Id).FirstOrDefaultAsync();
                DisLike disLike = await _context.DisLikes.Where(x => x.AppUserId == appUser.Id && x.ProductId == product.Id).FirstOrDefaultAsync();
                if (disLike == null)
                {
                    DisLike NewDislike = new DisLike()
                    {
                        ProductId = product.Id,
                        AppUserId = appUser.Id

                    };
                    _context.DisLikes.Add(NewDislike);
                    await _context.SaveChangesAsync();

                }
                else
                {
                    _context.DisLikes.Remove(disLike);
                    await _context.SaveChangesAsync();
                }

                if (like != null)
                {
                    _context.Likes.Remove(like);
                    await _context.SaveChangesAsync();
                }

            }



            return Content("ok");



        }
        public async Task<IActionResult> DeleteDisLike(int? id)
        {
            if (id == null) return BadRequest();

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();



            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name && !u.isAdmin);

            if (appUser != null)
            {
                
                DisLike disLike = await _context.DisLikes.Where(x => x.AppUserId == appUser.Id && x.ProductId == product.Id).FirstOrDefaultAsync();
                if (disLike != null)
                {
                    _context.DisLikes.Remove(disLike);
                    await _context.SaveChangesAsync();
                }
                

            }



            return Content("ok");



        }
        public async Task<IActionResult> DeleteLike(int? id)
        {
            if (id == null) return BadRequest();

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();



            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name && !u.isAdmin);

            if (appUser != null)
            {

                Like like = await _context.Likes.Where(x => x.AppUserId == appUser.Id && x.ProductId == product.Id).FirstOrDefaultAsync();
                if (like != null)
                {
                    _context.Likes.Remove(like);
                    await _context.SaveChangesAsync();
                }


            }



            return Content("ok");



        }
        public async Task<IActionResult> AddWishlist(int? id)
        {
            if (id == null) return BadRequest();

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();


            string cookieBasket = HttpContext.Request.Cookies["basket"];

            List<BasketVM> basketVMs = null;

            if (cookieBasket != null)
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(cookieBasket);

                if (basketVMs.Any(b => b.ProductId == id))
                {

                    return Content("no");
                }
               
            }
            



            string cookieWish = HttpContext.Request.Cookies["Wish"];

            List<WishVM> wishVMs = null;

            if (cookieWish != null)
            {
                wishVMs = JsonConvert.DeserializeObject<List<WishVM>>(cookieWish);

                if (wishVMs.Any(b => b.ProductId == id))
                {


                    wishVMs.Remove(wishVMs.Find(b => b.ProductId == id));
                }
                else
                {

                    wishVMs.Add(new WishVM
                    {
                        ProductId = (int)id,
                    });
                }
            }
            else
            {
                wishVMs = new List<WishVM>();
                wishVMs.Add(new WishVM
                {
                    ProductId = (int)id,
                });
            }

            cookieWish = JsonConvert.SerializeObject(wishVMs);
            HttpContext.Response.Cookies.Append("Wish", cookieWish);


            return Content("ok");



        }

        public IActionResult SearchProduct(string search)
        {


            

            IQueryable<Product> products = _context.Products
                .Include(x => x.ProductDetails).ThenInclude(x => x.DetailKey).ThenInclude(x=>x.DetailValues)
                .Include(r=>r.Reviews)
                .OrderByDescending(c => c.CreatedAt)
                .AsQueryable();

            if (search != null)
            {
                
                products = products.Where(c =>(c.Title.ToLower()+" "+c.Category.Name.ToLower()+" "+c.Brand.Name.ToLower()).Contains(search.ToLower())
                ||c.ProductDetails.Any(x=>x.DetailValue.Name.ToLower().Contains(search.ToLower()))
                
                ).Take(40);
            }
            else
            {
                products = products.Take(0);
            }

            return PartialView("_GetSearchList", products);

        }

        public  IActionResult CheckUserForReviews()
        {

            AppUser member = null;
            if (User.Identity.IsAuthenticated)
            {
                member = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name && !x.isAdmin);
            }
            if (member == null)
            {
                return Json(new { status = 400, message = $"" });
            }
            else
            {
                return Json(new { status = 200, message = $"" });
                //return PartialView("_RevFormPartial");
            }


        }
        public IActionResult GetFormReviews()
        {
            return PartialView("_RevFormPartial");
        }
        [HttpPost]
        public async Task<IActionResult> AddReviews([FromBody] ReviewVM review)
        {

            AppUser member = null;
            if (User.Identity.IsAuthenticated)
            {
                member = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            }
            if (member == null)
                return RedirectToAction("login", "account");

            Product product = await _context.Products
                .Where(x=>x.Id==review.ProductId)
                 .FirstOrDefaultAsync(x =>!x.IsDeleted);
            if (product == null) return NotFound();

            List<Review> reviews = await _context.Reviews.Where(x => x.ProductId == product.Id && x.AppUserId == member.Id).ToListAsync();
            

            if (reviews.Count>0)
            {
                return Json(new { status = 200, message = $"Çox hörmətli {member.Name} {member.SurName}  siz bu məhsul üçün  daha əvvəl rəy bildirmisiniz" });
            }

            Review SetReview = new Review {
                
                AppUserId = member.Id,
                Name=member.Name,
                Email=member.Email,
                ProductId = product.Id,
                Message =review.Message,
                Star=review.Star,
            
            };


            _context.Reviews.Add(SetReview);
            _context.SaveChanges();

            return Json(new { status = 400, message = $"" });
        }
        public async Task<IActionResult> DelReviews(int?id)
        {

            AppUser member = null;
            if (User.Identity.IsAuthenticated)
            {
                member = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            }
            if (member == null)
                return Json(new { status = 400, message = $"" });

            Review review = await _context.Reviews.Where(x => x.Id == id && x.AppUserId==member.Id).FirstOrDefaultAsync();
           


            _context.Reviews.Remove(review);
            _context.SaveChanges();

            return Json(new { status = 200, message = $"" });
        }
    }
}
