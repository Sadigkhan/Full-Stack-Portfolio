using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static Kontakt.Helpers.Helper;

namespace Kontakt.ViewModels
{
    public class UserInfoUpdateVM
    {
        [StringLength(255)]
        public string Name { get; set; }
        [StringLength(255)]
        public string ParentName { get; set; }
        public Nullable<DateTime> Birthday { get; set; }
        public Nullable<Gender> Gender { get; set; }

        [StringLength(255)]
        public string SurName { get; set; }

        [StringLength(255)]
        public string PhoneNumber { get; set; }
    }
}
