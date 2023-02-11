using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontakt.ViewModels
{
    public class ProductVsDetailVM
    {
        public DetailKeyVM DetailKey { get; set; }
        public DetailVm Detail { get; set; }
        public ProductVM product { get; set; }
    }
}
