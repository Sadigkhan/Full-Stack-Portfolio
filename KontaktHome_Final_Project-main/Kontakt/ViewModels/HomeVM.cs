using Kontakt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontakt.ViewModels
{
    public class HomeVM
    {
        public List<Product> Products { get; set; }
        public List<Product> DisProducts { get; set; }
        public List<Category> Categories { get; set; }
        public List<CategoryBrand> CategoryBrands { get; set; }
        public List<Brand> Brands { get; set; }
        public List<WishVM> WishVMs { get; set; }
    }
}
