using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DekorEvStartUpFinal.Models
{
    public class Slider:BaseEntity
    {
        [StringLength(1000)]
        public string LeftTopImage { get; set; }
        [StringLength(1000)]
        public string LeftBottomImage { get; set; }
        [StringLength(1000)]
        public string RightMainImage { get; set; }
        [NotMapped]
        public IFormFile LeftTopImageFile { get; set; }
        [NotMapped]
        public IFormFile LeftBottomImageFile { get; set; }
        [NotMapped]
        public IFormFile RightMainImageFile { get; set; }

    }
}
