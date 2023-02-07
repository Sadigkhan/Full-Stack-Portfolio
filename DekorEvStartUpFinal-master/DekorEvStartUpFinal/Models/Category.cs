using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DekorEvStartUpFinal.Models
{
    public class Category:BaseEntity
    {
        [StringLength(255),Required]
        public string Name { get; set; }
        [StringLength(1000)]
        public string CategoryImage { get; set; }
        [NotMapped]
        public IFormFile CategoryImageFile { get; set; }
        public bool IsMain { get; set; }
        public Nullable<int> ParentId { get; set; }
        public Category Parent { get; set; }
        public IEnumerable<Category> Children { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
