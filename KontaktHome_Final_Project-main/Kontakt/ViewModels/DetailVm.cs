using Kontakt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontakt.ViewModels
{
    public class DetailVm
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public Nullable<int> ProductId { get; set; }
        public Product Product { get; set; }
        public Nullable<int> DetailKeyId { get; set; }
        public DetailKey DetailKey { get; set; }
    }
}
