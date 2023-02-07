using DekorEvFinal.Helper;
using JuanBackFinal.DAL;
using JuanBackFinal.Extensions;
using JuanBackFinal.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JuanBackFinal.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class BlogController : Controller
    {
        public readonly JuanAppDbContext _context;
        public readonly IWebHostEnvironment _env;
        public BlogController(JuanAppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult>  Index(bool? status,int page=1)
        {

            ViewBag.Status = status;


            IEnumerable<Blog> blogs = await _context.Blogs
                .Include(b=>b.Publisher)
                .Include(b =>b.BlogTags).ThenInclude(b=>b.Tag)
                .Include(b=>b.BlogToCategories).ThenInclude(b=>b.BlogCategory)
                .Where(t => status != null ? t.IsDeleted == status : !t.IsDeleted)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();


            ViewBag.PageIndex = page;
            ViewBag.PageCount = Math.Ceiling((double)blogs.Count() / 5);

            return View(blogs.Skip((page-1)*5).Take(5));
        }

        public async Task<IActionResult> Detail(int?id,bool? status,int page=1)
        {
            if (id==null)
            {
                return BadRequest();
            }


            Blog blog = await _context.Blogs
                .Include(b => b.BlogToCategories).ThenInclude(bt => bt.BlogCategory)
                .Include(b => b.BlogTags).ThenInclude(bt => bt.Tag)
                .Include(b=>b.Publisher)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (blog == null) return NotFound();

            return View(blog);
        }
        public async Task<IActionResult> Create(bool ? status,int page=1)
        {
            ViewBag.BlogCategories = await _context.BlogCategories.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Tags = await _context.Tags.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Publishers = await _context.Publishers.Where(c => !c.IsDeleted).ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(Blog blog,bool? status,int page = 1)
        {
            ViewBag.BlogCategories = await _context.BlogCategories.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Tags = await _context.Tags.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Publishers = await _context.Publishers.Where(c => !c.IsDeleted).ToListAsync();

            if (!ModelState.IsValid)
            {
                return View();
            }
            if (blog.TagIds.Count > 0)
            {
                List<BlogTag> blogTags = new List<BlogTag>();

                foreach (int item in blog.TagIds)
                {
                    //if (!await _context.Tags.AnyAsync(t => t.Id != item && !t.IsDeleted))
                    //{
                    //    ModelState.AddModelError("TagIds", $"The tag which you choose: {item} - is wrong");
                    //    return View();
                    //}

                    if (!await _context.Tags.AnyAsync(t => t.Id == item && !t.IsDeleted))
                    {
                        ModelState.AddModelError("TagIds", $"The tag which you choose: {item} - is wrong");
                        return View();
                    }

                    BlogTag blogTag = new BlogTag
                    {
                        TagId = item
                    };

                    blogTags.Add(blogTag);
                }

                blog.BlogTags = blogTags;
            }


            if (blog.BlogCategoryIds.Count > 0)
            {
                List<BlogToCategory> blogToCategories = new List<BlogToCategory>();

                foreach (int item in blog.BlogCategoryIds)
                {
                    if (!await _context.BlogCategories.AnyAsync(t => t.Id == item && !t.IsDeleted))
                    {
                        ModelState.AddModelError("BlogCategoryIds", $"The category which you choose: {item} - is wrong");
                        return View();
                    }

                    BlogToCategory blogCategory = new BlogToCategory
                    {
                        BlogCategoryId = item
                    };

                    blogToCategories.Add(blogCategory);
                }

                blog.BlogToCategories = blogToCategories;
            }


            if (blog.BlogImageFile != null)
            {
                if (!blog.BlogImageFile.CheckFileContentType("image/"))
                {
                    ModelState.AddModelError("BlogImageFile", "Only image type is allowed");
                    return View();
                }
                if (!blog.BlogImageFile.CheckFileSize(100))
                {
                    ModelState.AddModelError("BlogImageFile", "Image size can't be more than 100Kb");
                    return View();
                }
                blog.BlogImage = blog.BlogImageFile.CreateFile(_env, "assets", "img", "blog");
            }
            else
            {
                ModelState.AddModelError("BlogImageFile", "Main Image is required");
                return View();
            }

            blog.CreatedAt = DateTime.UtcNow.AddHours(4);

            await _context.Blogs.AddAsync(blog);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { status, page });
        }

        public async Task<IActionResult> Update(int? id, bool? status, int page = 1)
        {
            ViewBag.BlogCategories = await _context.BlogCategories.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Tags = await _context.Tags.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Publishers = await _context.Publishers.Where(c => !c.IsDeleted).ToListAsync();

            if (id == null) return BadRequest();
            Blog blog = await _context.Blogs
               .Include(b => b.BlogToCategories).ThenInclude(bt => bt.BlogCategory)
               .Include(b => b.BlogTags).ThenInclude(bt => bt.Tag)
               .Include(b => b.Publisher)
               .FirstOrDefaultAsync(b => b.Id == id&&!b.IsDeleted);

            if (blog == null) return NotFound();



            blog.TagIds = blog.BlogTags.Select(pt => pt.Tag.Id).ToList();
            blog.BlogCategoryIds = blog.BlogToCategories.Select(pt => pt.BlogCategory.Id).ToList();

            return View(blog);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Blog blog, bool? status, int page = 1)
        {
            ViewBag.BlogCategories = await _context.BlogCategories.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Tags = await _context.Tags.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Publishers = await _context.Publishers.Where(c => !c.IsDeleted).ToListAsync();

            if (id == null) return BadRequest();

            if (id != blog.Id)
            {
                return BadRequest();
            }

            Blog dbBlog = await _context.Blogs
              .Include(b => b.BlogToCategories).ThenInclude(bt => bt.BlogCategory)
              .Include(b => b.BlogTags).ThenInclude(bt => bt.Tag)
              .Include(b => b.Publisher)
              .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);
            if (dbBlog == null) return NotFound();

            if (!ModelState.IsValid) return View(dbBlog);

            if (!await _context.Publishers.AnyAsync(b => b.Id == blog.PublisherId && !b.IsDeleted))
            {
                ModelState.AddModelError("PublisherId", "Please choose right publisher ");
                return View(dbBlog);
            }

            if (blog.BlogCategoryIds.Count > 0)
            {
                _context.BlogToCategories.RemoveRange(dbBlog.BlogToCategories);

                List<BlogToCategory> blogCategories = new List<BlogToCategory>();

                foreach (int item in blog.BlogCategoryIds)
                {
                    if (!await _context.BlogCategories.AnyAsync(t => t.Id == item && !t.IsDeleted))
                    {
                        ModelState.AddModelError("BlogCategoryIds", $"Choosen Blog Category Id {item} - is wrong");
                        return View(dbBlog);
                    }

                    BlogToCategory blogCategory = new BlogToCategory
                    {
                        BlogCategoryId = item
                    };

                    blogCategories.Add(blogCategory);
                }

                dbBlog.BlogToCategories = blogCategories;
            }
            else
            {
                _context.BlogToCategories.RemoveRange(dbBlog.BlogToCategories);
            }

            if (blog.TagIds.Count > 0)
            {
                _context.BlogTags.RemoveRange(dbBlog.BlogTags);


                List<BlogTag> blogTags = new List<BlogTag>();

                foreach (int item in blog.TagIds)
                {
                    if (!await _context.BlogTags.AnyAsync(t => t.Id == item && !t.IsDeleted))
                    {
                        ModelState.AddModelError("TagIds", $"Choosen Tag Category Id {item} - is wrong");
                        return View(dbBlog);
                    }

                    BlogTag blogTag = new BlogTag
                    {
                        TagId = item
                    };

                    blogTags.Add(blogTag);
                }

                dbBlog.BlogTags = blogTags;
            }
            else
            {
                _context.BlogTags.RemoveRange(dbBlog.BlogTags);
            }

            if (blog.BlogImageFile != null)
            {
                if (!blog.BlogImageFile.CheckFileContentType("image/"))
                {
                    ModelState.AddModelError("BlogImageFile", "Only image type is allowed");
                    return View();
                }
                if (!blog.BlogImageFile.CheckFileSize(100))
                {
                    ModelState.AddModelError("BlogImageFile", "Image size can't be more than 100Kb");
                    return View();
                }
                Helper.DeleteFile(_env, dbBlog.BlogImage, "assets", "img", "blog");
                dbBlog.BlogImage = blog.BlogImageFile.CreateFile(_env, "assets", "img", "blog");
            }
            dbBlog.Title=blog.Title;
            dbBlog.Description=blog.Description;
            dbBlog.PublisherId= blog.PublisherId;
            dbBlog.UpdatedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return RedirectToAction("index", new { status, page });



        }



        public async Task<IActionResult> Delete(int? id, bool? status, int page = 1)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Blog dbBlog = await _context.Blogs
                        .Include(b => b.BlogToCategories).ThenInclude(bt => bt.BlogCategory)
                        .Include(b => b.BlogTags).ThenInclude(bt => bt.Tag)
                        .Include(b => b.Publisher)
                        .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);
            if (dbBlog == null)
            {
                return NotFound();
            }
            dbBlog.IsDeleted = true;
            dbBlog.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return RedirectToAction("index", new { status, page });
        }
    }
}
