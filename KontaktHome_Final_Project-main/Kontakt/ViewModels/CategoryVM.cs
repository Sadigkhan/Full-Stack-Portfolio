using Kontakt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontakt.ViewModels
{
    public class CategoryVM
    {
        public Category Category { get; set; }
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }


    }
}
