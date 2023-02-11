using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kontakt.Models
{
    public class Review:BaseEntity
    {
        [StringLength(255), Required]
        public string Name { get; set; }
        [StringLength(255), Required]
        public string Email { get; set; }
        [StringLength(1000), Required]
        public string Message { get; set; }
        [Required, Range(1, 5)]
        public int Star { get; set; }
        public int? ProductId { get; set; }
        public Product Product { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
