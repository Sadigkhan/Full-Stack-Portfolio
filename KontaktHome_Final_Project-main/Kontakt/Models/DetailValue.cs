using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Kontakt.Models
{
    public class DetailValue:BaseEntity
    {
        [StringLength(255)]
        public string Name { get; set; }
        [NotMapped]
        public List<int> DetailKeyIds { get; set; } = new List<int>();
        public Nullable<int> DetailKeyId { get; set; }
        public DetailKey DetailKey { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Category Category { get; set; }
        //public Nullable<int> BrandId { get; set; }
        //public Brand Brand { get; set; }
        public IEnumerable<ProductDetail> ProductDetails { get; set; }
        [NotMapped]
        public List<string> Names { get; set; } = new List<string>();

    }
}
