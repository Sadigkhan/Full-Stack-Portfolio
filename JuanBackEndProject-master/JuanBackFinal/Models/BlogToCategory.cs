using System;

namespace JuanBackFinal.Models
{
    public class BlogToCategory
    {
        public int Id { get; set; } 
        public Nullable<int> BlogId { get; set; }
        public Blog Blog { get; set; }
        public Nullable<int> BlogCategoryId { get; set; }
        public BlogCategory BlogCategory { get; set; }
    }
}
