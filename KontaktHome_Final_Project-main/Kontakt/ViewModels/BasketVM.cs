using Kontakt.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Kontakt.ViewModels
{
    public class BasketVM
    {
        public int ProductId { get; set; }
        public string Title { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public List<Review> Reviews { get; set; }
        public Product Product { get; set; }
    }
}
