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
    public class UserUpdateVM
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
        public string UserName { get; set; }
        [StringLength(255), EmailAddress]
        public string Email { get; set; }
        [StringLength(255)]
        public string PhoneNumber { get; set; }
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

        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare(nameof(Password)), DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
