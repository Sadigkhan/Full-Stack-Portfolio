using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kontakt.ViewModels
{
    public class ReviewVM
    {
        [StringLength(1000), Required]
        public string Message { get; set; }
        [Required, Range(1, 5)]
        public int Star { get; set; }
        public int? ProductId { get; set; }
    }
}
