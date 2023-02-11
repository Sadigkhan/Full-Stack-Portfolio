using Kontakt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontakt.ViewModels
{
    public class ProductDetailVM
    {
        public Product Product { get; set; }
        public List<Product> Products { get; set; }
        public Review Review { get; set; }
        public CategoryBrand CategoryBrand { get; set; }
    }
}
