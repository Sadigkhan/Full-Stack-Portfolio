using DekorEvStartUpFinal.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DekorEvStartUpFinal.DAL
{
    public class DekorEvStartupAppDbContext:IdentityDbContext<AppUser>
    {
        public DekorEvStartupAppDbContext(DbContextOptions<DekorEvStartupAppDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage>ProductImages { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Color>Colors { get; set; }
        public DbSet<Material>Materials { get; set; }
        public DbSet<ProductColorMaterial>ProductColorMaterials { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ViewCount> ViewCounts { get; set; }
        public DbSet<Compare> Compares { get; set; }

    }
}
