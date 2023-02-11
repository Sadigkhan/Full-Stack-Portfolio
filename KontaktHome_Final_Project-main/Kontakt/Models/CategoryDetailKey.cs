using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Kontakt.Models
{
    public class CategoryDetailKey
    {
        public int Id { get; set; }
        
        public Nullable<int> CategoryId { get; set; }
        public Category Category { get; set; }
        public Nullable<int> DetailKeyId { get; set; }
        public DetailKey DetailKey { get; set; }
        // [NotMapped]
        //public DetailValue DetailValue { get; set; }

    }
}
