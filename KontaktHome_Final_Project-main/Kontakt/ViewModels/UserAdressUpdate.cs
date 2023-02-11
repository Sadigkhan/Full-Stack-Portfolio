using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static Kontakt.Helpers.Helper;

namespace Kontakt.ViewModels
{
    public class UserAdressUpdate
    {

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

      
    }
}
