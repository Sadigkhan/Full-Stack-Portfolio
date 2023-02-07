using DekorEvStartUpFinal.Models;
using System.Collections.Generic;

namespace DekorEvStartUpFinal.ViewModels.Market
{
    public class MarketDetailVM
    {
        public IEnumerable<Product> Products { get; set; }
        public ViewCount ViewCounts { get; set; }
        public AppUser User { get; set; }
    }
}
