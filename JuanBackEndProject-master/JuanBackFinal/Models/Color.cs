using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JuanBackFinal.Models
{
    public class Color:BaseEntity
    {
        [StringLength(255),Required]
        public string Name { get; set; }
        public IEnumerable<ProductColor> ProductColors { get; set; }
    }
}
