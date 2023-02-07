using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DekorEvStartUpFinal.Models
{
    public class Setting:BaseEntity
    {
        [StringLength(1000)]
        public string Logo { get; set; }
        [StringLength(1000)]
        public string Offer { get; set; }
        [StringLength(100), Required, EmailAddress]
        public string Email { get; set; }
        [StringLength(100), Required]
        public string PhoneNumber { get; set; }
        [StringLength(255), Required]
        public string Adress { get; set; }
        [StringLength(5000)]
        public string AboutUs { get; set; }
        [StringLength(1000)]
        public string AboutUsImage { get; set; }
        [NotMapped]
        public IFormFile AboutUsImageFile { get; set; }
        [NotMapped]
        public IFormFile LogoImage { get; set; }
    }
}
