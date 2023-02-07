using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JuanBackFinal.Models
{
    public class Tag:BaseEntity
    {
        [StringLength(255),Required]
        public string Name { get; set; }
        public IEnumerable<BlogTag> BlogTags { get; set; }
    }
}
