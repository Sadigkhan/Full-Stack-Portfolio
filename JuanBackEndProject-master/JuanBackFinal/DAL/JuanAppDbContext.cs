using JuanBackFinal.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JuanBackFinal.DAL
{
    public class JuanAppDbContext : IdentityDbContext<AppUser>
    {
        public JuanAppDbContext(DbContextOptions<JuanAppDbContext> options) : base(options) { }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<ServiceOffer> ServiceOffers { get; set; }
        public DbSet<Slider> Sliders { get; set; } 
        public DbSet<Brand>Brands { get; set; }
        public DbSet<Category>Categories { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductColor>ProductColors { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }
        public DbSet<Blog>Blogs { get; set; }
        public DbSet<Tag>Tags { get; set; }
        public DbSet<BlogTag> BlogTags { get; set; }
        public DbSet<BlogCategory>BlogCategories { get; set; }
        public DbSet<BlogToCategory>BlogToCategories { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Banner>Banners { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }


    }
}
