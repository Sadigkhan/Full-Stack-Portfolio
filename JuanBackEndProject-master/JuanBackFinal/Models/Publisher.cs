using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JuanBackFinal.Models
{
    public class Publisher:BaseEntity
    {
        [StringLength(255),Required]
        public string PublisherName { get; set; }
        [StringLength(255),Required]
        public string PublisherPosition { get; set; }
        [StringLength(1000)]
        public string PublisherImage { get; set; }
        [NotMapped]
        public IFormFile PublisherImageFile { get; set; }
        public IEnumerable<Blog> Blogs { get; set; }

    }
}
