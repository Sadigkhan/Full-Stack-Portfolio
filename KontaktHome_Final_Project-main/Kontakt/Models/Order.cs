using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static Kontakt.Helpers.Helper;

namespace Kontakt.Models
{
    public class Order:BaseEntity
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public double TotalPrice { get; set; }
        [StringLength(255), Required]
        public string Address { get; set; }
        [StringLength(255), Required]
        public string Country { get; set; }
        [StringLength(255), Required]
        public string City { get; set; }
        [StringLength(255), Required]
        public string State { get; set; }
        [StringLength(255), Required]
        public string ZipCode { get; set; }
        [StringLength (1000)]
        public string OrderNote { get; set; }
        public OrderStatus Status { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; }
    }
}
