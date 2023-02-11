using Kontakt.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontakt.DAL
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<CategoryBrand> CategoryBrands { get; set; }
        public DbSet<DetailKey> DetailKeys { get; set; }
        public DbSet<ProductDetail> ProductDetails { get; set; }
        public DbSet<CategoryDetailKey> CategoryDetailKeys { get; set; }
        public DbSet<DetailValue> DetailValues { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<DisLike> DisLikes { get; set; }
    }
}
