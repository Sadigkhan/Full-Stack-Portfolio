using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JuanBackFinal.Models
{
    public class Blog : BaseEntity
    {
        [StringLength(1000)]
        public string BlogImage { get; set; }
        [StringLength(255),Required]
        public string Title { get; set; }
        [StringLength(2000),Required]
        public string Description { get; set; }
        [NotMapped]
        public IFormFile BlogImageFile { get; set; }
        public IEnumerable<BlogTag> BlogTags { get; set; }
        public IEnumerable<BlogToCategory> BlogToCategories { get; set; }
        public Nullable<int> PublisherId { get; set; }
        public Publisher Publisher { get; set; }

        [NotMapped]
        public List<int> TagIds { get; set; } = new List<int>();
        [NotMapped]
        public List<int> BlogCategoryIds { get; set; } = new List<int>();
    }
}
