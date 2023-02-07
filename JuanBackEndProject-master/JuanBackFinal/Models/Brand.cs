using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JuanBackFinal.Models
{
    public class Brand:BaseEntity
    {
        [StringLength(1000)]
        public string BrandImage { get; set; }
        [NotMapped]
        public IFormFile BrandImageFile { get; set; }
    }
}
