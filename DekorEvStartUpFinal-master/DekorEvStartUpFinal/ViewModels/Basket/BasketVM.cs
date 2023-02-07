using DekorEvStartUpFinal.Models;

namespace DekorEvStartUpFinal.ViewModels.Basket
{
    public class BasketVM
    {
        public int Count { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public int ProductId { get; set; }
        public int ColorId { get; set; }
        public int MaterialId { get; set; }
        public ProductColorMaterial productColorMaterial { get; set; }

    }
}
