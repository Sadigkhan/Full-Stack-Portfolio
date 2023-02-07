using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DekorEvStartUpFinal.Models
{
    public class Color:BaseEntity
    {
        [StringLength(255),Required]
        public string Name { get; set; }
        public IEnumerable<Basket> Baskets { get; set; }  
        public IEnumerable<Compare> Compares { get; set; }

        public IEnumerable<ProductColorMaterial> ProductColorMaterials { get; set; }


    }
}
