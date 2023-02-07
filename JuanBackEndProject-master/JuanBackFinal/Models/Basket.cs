using System;

namespace JuanBackFinal.Models
{
    public class Basket:BaseEntity
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public Nullable<int> ProductId { get; set; }
        public Product Product { get; set; }
        public int Count { get; set; }
    }
}
