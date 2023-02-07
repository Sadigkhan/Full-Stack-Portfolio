using System;

namespace DekorEvStartUpFinal.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
        public Nullable<int> ProductId { get; set; }
        public Product Product { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
