using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JuanBackFinal.Models
{
    public class Size:BaseEntity
    {
      [StringLength(255),Required]
      public string Name { get; set; }
       public IEnumerable<ProductSize> ProductSizes { get; set; }
    }
}
