using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kontakt.ViewModels
{
    public class UserAuthUpdateVM
    {
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare(nameof(Password)), DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [StringLength(255), EmailAddress]
        public string Email { get; set; }
    }
}
