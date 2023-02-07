using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DekorEvStartUpFinal.Models
{
    public class Product:BaseEntity
    {   [StringLength(255),Required]
        public string Name { get; set; }
        [StringLength(5000),Required]
        public string Description { get; set; }
        [Required,Column(TypeName ="money")]
        public double CostPrice { get; set; }
        [Required, Column(TypeName = "money")]
        public double SalePrice { get; set; }
        [Required, Column(TypeName = "money")]
        public double DiscountPrice { get; set; }
        public string ProductCode { get; set; }
        public int Count { get; set; }
        [StringLength(1000)]
        public string MainImage { get; set; }

        public List<ProductImage> ProductImages { get; set; }

        [NotMapped]
        public List<IFormFile> ImageFiles { get; set; } = new List<IFormFile>();
        public bool IsAvailable { get; set; }
        public bool IsVip { get; set; }
        public bool IsPremium { get; set; }
        public bool IsFronted { get; set; }
        public bool IsNew { get; set; }
        public bool IsBestSeller { get; set; }
        public bool IsDeliverable { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Category Category { get; set; }

        public List<ProductColorMaterial> ProductColorMaterials { get; set; }
        [NotMapped]
        public List<int> ColorIds { get; set; } = new List<int>();
        [NotMapped]
        public List<int> MaterialIds { get; set; } = new List<int>();
        [NotMapped]
        public List<int> Counts { get; set; } = new List<int>();

        public IEnumerable<Basket> Baskets { get; set; }
        public IEnumerable<Compare> Compares { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        [NotMapped]
        public int ParentId { get; set; }
        public ViewCount ViewCount { get; set; }
        public bool DeletedByAdmin { get; set; }
        public Nullable<DateTime> VipPaymentDate { get; set; }
        public Nullable<DateTime> FrontedPaymentDate { get; set; }

        public Nullable<DateTime> PremiumPaymentDate { get; set; }


    }
}
