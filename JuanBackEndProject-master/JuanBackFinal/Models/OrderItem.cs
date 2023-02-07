using System;

namespace JuanBackFinal.Models
{
    public class OrderItem : BaseEntity
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public Nullable<int> ProductId { get; set; }
        public Product Product { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }
        public double TotalPrice { get; set; }
    }
}
