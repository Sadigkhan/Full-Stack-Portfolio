using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Kontakt.Models
{
    public class Category:BaseEntity
    {
        [StringLength(255), Required]
        public string Name { get; set; }
        [StringLength(1000)]
        public string ImageUrl { get; set; }
        public bool IsMain { get; set; }
        [NotMapped]
        public IFormFile CategoryImage { get; set; }
        [NotMapped]
        public List<int> BrandIds { get; set; } = new List<int>();
        public Nullable<int> ParentId { get; set; }
        public Category Parent { get; set; }
        public IEnumerable<Category> Children { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<CategoryBrand> CategoryBrands { get; set; }
        public IEnumerable<CategoryDetailKey> CategoryDetailKeys { get; set; }
        public IEnumerable<DetailValue> DetailValues { get; set; }



    }
}
