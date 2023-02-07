using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JuanBackFinal.Models
{
    public class Slider:BaseEntity
    {
        [StringLength(255)]
        public string Subtitle { get; set; }
        [StringLength(255)]
        public string Title { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
        [StringLength(1000)]
        public string SliderImage { get; set; }
        [NotMapped]
        public IFormFile SliderImageFile { get; set; }
    }
}
