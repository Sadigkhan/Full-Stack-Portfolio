using System;

namespace DekorEvStartUpFinal.Models
{
    public class Compare:BaseEntity
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public Nullable<int> ColorId { get; set; }
        public Color Color { get; set; }
        public Nullable<int> MaterialId { get; set; }
        public Material Material { get; set; }
        public Nullable<int> ProductId { get; set; }
        public Product Product { get; set; }
        //public int Count { get; set; }

    }
}
