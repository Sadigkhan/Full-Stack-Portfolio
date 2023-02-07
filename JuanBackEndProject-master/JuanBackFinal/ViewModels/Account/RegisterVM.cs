using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JuanBackFinal.ViewModels.Account
{
    public class RegisterVM
    {
        [StringLength(255),Required]
        public string FullName { get; set; }
        [Required]
        public string UserName { get; set; }
        [EmailAddress, Required]
        public string Email { get; set; }
        [Required,DataType(DataType.Password)]
        public string Password { get; set; }
        [Required,Compare(nameof(Password)), DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
