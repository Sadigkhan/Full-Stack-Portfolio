using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DekorEvStartUpFinal.ViewModels.Account
{
    public class RegisterVM
    {

        [StringLength(255), Required]
        public string FullName { get; set; }
        [Required]
        public string UserName { get; set; }
        [EmailAddress, Required]
        public string Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, Compare(nameof(Password)), DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public bool Terms { get; set; }
        [StringLength(maximumLength:5000)]
        public string Description { get; set; }
        public IFormFile UserImageFile { get; set; }

    }
}
