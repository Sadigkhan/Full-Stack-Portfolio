using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Kontakt.Models
{
    public class DetailKey:BaseEntity
    {
        [StringLength(255), Required]
        public string Name { get; set; }
        public bool ForTitle { get; set; }
        public bool isMain { get; set; }
        [NotMapped]
        public List<int> CategoryIds { get; set; } = new List<int>();
        public IEnumerable<ProductDetail> ProductDetails { get; set; }
        public IEnumerable<CategoryDetailKey> CategoryDetailKeys { get; set; }
        
        public IEnumerable<DetailValue> DetailValues { get; set; }
    }
}
