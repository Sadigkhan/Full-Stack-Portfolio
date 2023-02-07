using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JuanBackFinal.Models
{
    public class BlogCategory:BaseEntity
    {
        [StringLength(255), Required]
        public string Name { get; set; }
        public IEnumerable<BlogToCategory> BlogToCategories { get; set; }
    }
}
