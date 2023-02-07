using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JuanBackFinal.Models
{
    public class Banner:BaseEntity
    {
        [StringLength(255)]
        public string SubTitle { get; set; }
        [StringLength(255)]
        public string Title { get; set; }
        [StringLength(1000)]
        public string BannerImage { get; set; }
        [NotMapped]
        public IFormFile BannerImageFile { get; set; }
    }
}
