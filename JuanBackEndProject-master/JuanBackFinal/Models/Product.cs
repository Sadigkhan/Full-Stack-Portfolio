using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JuanBackFinal.Models
{
    public class Product:BaseEntity
    {
        [StringLength(255),Required]
        public string Name { get; set; }
        [StringLength(500),Required]
        public string Description { get; set; }
        [StringLength(1000)]
        public string MainImage { get; set; }
        [Column(TypeName ="money"),Required]
        public double SalePrice { get; set; }
        [Column(TypeName = "money"),Required]
        public double CostPrice { get; set; }
        [Column(TypeName = "money"),Required]
        public double DiscountPrice { get; set; }
        [Required]
        public int Count { get; set; }
        public bool IsAvailable { get; set; }
        public bool TopSeller { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Category Category { get; set; }
        [NotMapped]
        public IFormFile MainImageFile { get; set; }
        public IEnumerable<ProductColor> ProductColors { get; set; }
        public IEnumerable<ProductSize> ProductSizes { get; set; }
        [NotMapped]
        public List<int> ColorIds { get; set; }=new List<int>();
        [NotMapped]
        public List<int> SizeIds { get; set; }=new List<int>();
        public List<ProductImage> ProductImages { get; set; }
        [NotMapped]
        public IFormFile[] ProductimageFiles { get; set; }

        public IEnumerable<OrderItem> OrderItems { get; set; }
        public IEnumerable<Basket> Baskets { get; set; }
        public IEnumerable<Review> Reviews { get; set; }

    }
}
