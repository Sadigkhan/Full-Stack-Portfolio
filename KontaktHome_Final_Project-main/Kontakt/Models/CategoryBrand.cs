using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Kontakt.Models
{
    public class CategoryBrand:BaseEntity
    {
       
        public string ImageUrl { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Category Category { get; set; }
        public Nullable<int> BrandId { get; set; }
        public Brand Brand { get; set; }
    }
}
