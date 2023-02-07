using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DekorEvStartUpFinal.Models
{
    public class Material:BaseEntity
    {
        [StringLength(255)]
        public string Name { get; set; }
        public IEnumerable<Basket> Baskets { get; set; }
        public IEnumerable<Compare> Compares { get; set; }

        public IEnumerable<ProductColorMaterial> ProductColorMaterials { get; set; }

    }
}
