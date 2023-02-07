using System.ComponentModel.DataAnnotations;

namespace JuanBackFinal.Models
{
    public class ProductImage:BaseEntity
    {

        [StringLength(1000)]
        public string Image { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
