using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JuanBackFinal.Models
{
    public class Setting:BaseEntity
    {
        [StringLength(1000)]
        public string Logo { get; set; }
        [StringLength(1000)]
        public string Greeting { get; set; }
        [StringLength(1000),Required]
        public string Adress { get; set; }
        [StringLength(1000),Required,EmailAddress]
        public string Email { get; set; }
        [StringLength(1000),Required]
        public string PhoneNumber { get; set; }
        [StringLength(1000),Required]
        public string ContactUsTitle { get; set; }
        [StringLength(1000),Required]
        public string ContactUsDescription { get; set; }
        [StringLength(1000),Required]
        public string WorkHours { get; set; }
        [NotMapped]
        public IFormFile LogoFile { get; set; }
    }
}
