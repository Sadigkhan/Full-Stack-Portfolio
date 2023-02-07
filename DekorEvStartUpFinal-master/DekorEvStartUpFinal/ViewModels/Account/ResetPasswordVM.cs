using System.ComponentModel.DataAnnotations;

namespace DekorEvStartUpFinal.ViewModels.Account
{
    public class ResetPasswordVM
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare(nameof(Password)), DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
