using System.ComponentModel.DataAnnotations;

namespace Andrei_Mikhaleu_Task1.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Enter your login")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Enter your password")]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
