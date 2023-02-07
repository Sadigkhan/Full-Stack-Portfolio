using JuanBackFinal.Models;
using System.Collections.Generic;

namespace JuanBackFinal.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Slider> Sliders { get; set; }
        public IEnumerable<ServiceOffer> ServiceOffers { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Banner> Banners { get; set; }
        public IEnumerable<Blog> Blogs { get; set; }
        public IEnumerable<Brand> Brands { get; set; }
        public Setting Settings { get; set; }
        public IEnumerable<Category>Categories{get;set;}

         
    }
}
