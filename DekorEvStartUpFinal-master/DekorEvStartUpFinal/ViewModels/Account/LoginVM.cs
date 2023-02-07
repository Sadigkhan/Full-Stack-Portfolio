using System.ComponentModel.DataAnnotations;

namespace DekorEvStartUpFinal.ViewModels.Account
{
    public class LoginVM
    {
        [EmailAddress, Required]
        public string Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }

    }
}
