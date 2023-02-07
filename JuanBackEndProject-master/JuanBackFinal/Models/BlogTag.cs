using System;

namespace JuanBackFinal.Models
{
    public class BlogTag:BaseEntity
    {
        public int Id { get; set; }
        public Nullable<int> BlogId { get; set; }
        public Blog  Blog { get; set; }
        public Nullable<int> TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
