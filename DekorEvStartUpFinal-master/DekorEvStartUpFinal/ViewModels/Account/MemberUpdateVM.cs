using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DekorEvStartUpFinal.ViewModels.Account
{
    public class MemberUpdateVM
    {
        [StringLength(255),Required]
        public string FullName { get; set; }
        [StringLength(255),Required]

        public string UserName { get; set; }
        [StringLength(255),Required,EmailAddress]
        public string Email { get; set; }
        [StringLength(255)]
        public string PhoneNumber { get; set; }
        [StringLength(255)]
        public string Adress { get; set; }
        [StringLength(255)]
        public string City { get; set; }
        [StringLength(5000)]
        public string Description { get; set; }
        [StringLength(1000)]
        public string UserImage { get; set; }
        public IFormFile UserImageFile { get; set; }
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare(nameof(Password)), DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
