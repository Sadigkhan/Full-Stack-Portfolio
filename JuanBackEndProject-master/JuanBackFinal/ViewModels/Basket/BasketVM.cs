using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JuanBackFinal.ViewModels.Basket
{
    public class BasketVM
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public double ExTax { get; set; }
    }
}
