using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DekorEvStartUpFinal.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public double TotalPrice { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public bool Status { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public List<OrderItem>OrderItems { get; set; }
    }
}
