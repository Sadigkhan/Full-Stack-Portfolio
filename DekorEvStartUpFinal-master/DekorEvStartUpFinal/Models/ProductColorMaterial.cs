using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DekorEvStartUpFinal.Models
{
    public class ProductColorMaterial:BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public Nullable<int> MaterialId { get; set; }
        public Material Material { get; set; }
        public Nullable<int> ColorId { get; set; }
        public Color Color { get; set; }
        [Required]
        public int Count { get; set; }
        [StringLength(1000)]
        public string Image { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }

    }
}
