using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kontakt.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Ad məcburidir"), MaxLength(100)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Soyad məcburidir"), MaxLength(100)]
        public string SurName { get; set; }
        [Required(ErrorMessage = "Username məcburidir")]
        public string UserName { get; set; }
        [Required (ErrorMessage ="Email məcburidir"), MaxLength(255),DataType(DataType.EmailAddress,ErrorMessage ="Ancaq Email daxil edile biler")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Şifrə məcburidir"), MaxLength(255), DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Təkrar şifrə məcburidir"), MaxLength(255), DataType(DataType.Password), Compare(nameof(Password),ErrorMessage ="Şifrələr eyni deyil")]
        public string CheckPassword { get; set; }
    }
}
