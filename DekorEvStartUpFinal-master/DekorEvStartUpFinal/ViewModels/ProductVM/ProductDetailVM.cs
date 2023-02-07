using DekorEvStartUpFinal.Models;
using System.Collections.Generic;

namespace DekorEvStartUpFinal.ViewModels.ProductVM
{
    public class ProductDetailVM
    {
        public List<Product> Products { get; set; }
        public Product Product { get; set; }
        public AppUser appUser { get; set; }
        public ProductColorMaterial productColorMaterial { get; set; }
        public ViewCount ViewCounts { get; set; }

    }
}
