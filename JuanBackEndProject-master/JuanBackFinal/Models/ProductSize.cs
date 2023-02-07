using System;

namespace JuanBackFinal.Models
{
    public class ProductSize
    {
        public int Id { get; set; }
        public Nullable<int> SizeId { get; set; }
        public Size Size { get; set; }
        public Nullable<int> ProductId { get; set; }
        public Product Product { get; set; }

    }
}
