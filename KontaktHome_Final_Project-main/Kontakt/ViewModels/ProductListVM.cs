using Kontakt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontakt.ViewModels
{
    public class ProductListVM
    {
        public List<Product> Products { get; set; }
        public Category Category { get; set; }
        public List<CategoryDetailKey> CategoryDetailKeys { get; set; }
        public Brand Brand { get; set; }
        public CategoryBrand CategoryBrand { get; set; }
    }
}
