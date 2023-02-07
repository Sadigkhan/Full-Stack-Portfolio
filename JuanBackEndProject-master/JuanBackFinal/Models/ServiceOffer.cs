using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JuanBackFinal.Models
{
    public class ServiceOffer:BaseEntity
    {
        [StringLength(1000)]
        public string Image { get; set; }
        [StringLength(100)]
        public string Title { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
