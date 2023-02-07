using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JuanBackFinal.Models
{
    public class Category:BaseEntity
    {
        [StringLength(255),Required]
        public string Name { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
