using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Andrei_Mikhaleu_Task1.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [MinLength(4, ErrorMessage = "Username must be at least 4 characters long")]
        public string UserName { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).+$",
            ErrorMessage = "Password must contain at least one uppercase and one lowercase letter, one digit and special character.")]
        public string Password { get; set; }
    }
}
