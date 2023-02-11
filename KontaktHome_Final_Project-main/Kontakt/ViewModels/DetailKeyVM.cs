using Kontakt.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Kontakt.ViewModels
{
    public class DetailKeyVM:BaseEntity
    {
     
        public string Name { get; set; }
        [NotMapped]
        public List<int> CategoryIds { get; set; } = new List<int>();
        public IEnumerable<ProductDetail> ProductDetails { get; set; }
        public IEnumerable<CategoryDetailKey> CategoryDetailKeys { get; set; }
    }
}
