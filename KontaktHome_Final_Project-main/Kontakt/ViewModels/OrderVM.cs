using Kontakt.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kontakt.ViewModels
{
    public class OrderVM
    {
        [StringLength(255),  EmailAddress]
        public string Email { get; set; }
        [StringLength(255)]
        public string Name { get; set; }
        [StringLength(255)]
        public string Surname { get; set; }
        public string ParentName { get; set; }
        [StringLength(255)]
        public string Address { get; set; }
        [StringLength(255)]
        public string Country { get; set; }
        [StringLength(255)]
        public string City { get; set; }
        [StringLength(255)]
        public string State { get; set; }
        [StringLength(255)]
        public string ZipCode { get; set; }
        public string Phone { get; set; }

        [StringLength(255), EmailAddress]
        public string Note { get; set; }
        public List<Basket> Baskets { get; set; }
        public List<BasketVM> BasketVMs { get; set; }

    }
}
