using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DekorEvStartUpFinal.ViewModels.Order
{
    public class OrderVM
    {
        [StringLength(255),Required,EmailAddress]
        public string Email { get; set; }
        [StringLength(255),Required]
        public string FullName { get; set; }
        [StringLength(255), Required]
        public string City { get; set; }
        [StringLength(255), Required]
        public string Address { get; set; }
        public List< DekorEvStartUpFinal.Models.Basket> Baskets { get; set; }
    }
}
