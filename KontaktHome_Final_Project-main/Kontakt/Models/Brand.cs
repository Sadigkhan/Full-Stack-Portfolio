using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Kontakt.Models
{
    public class Brand:BaseEntity
    {
        [StringLength(255), Required]
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<CategoryBrand> CategoryBrands { get; set; }
        public IEnumerable<DetailValue> DetailValues { get; set; }
    }
}
