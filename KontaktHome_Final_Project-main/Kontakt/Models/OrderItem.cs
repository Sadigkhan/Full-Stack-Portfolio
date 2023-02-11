using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontakt.Models
{
    public class OrderItem:BaseEntity
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }
      
        public double Price { get; set; }
        public int Count { get; set; }
        public double TotalPrice { get; set; }
        public Nullable<int> ProductId { get; set; }

        public Product Product { get; set; }
        
    }
}
