using DekorEvStartUpFinal.Models;
using System.Collections.Generic;

namespace DekorEvStartUpFinal.ViewModels.Home
{
    public class HomeVM
    {
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Product> IsVip { get; set; }
        public IEnumerable<Product> IsNew { get; set; }
        public IEnumerable<Product> IsPremium { get; set; }
        public IEnumerable<Slider> Sliders { get; set; }
    }
}
