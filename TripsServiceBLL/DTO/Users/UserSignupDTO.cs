using System.ComponentModel.DataAnnotations;

namespace TripsServiceBLL.DTO.Users
{
    public class UserSignupDTO
    {
        [Required(ErrorMessage = "Enter your username")]
        [MinLength(4, ErrorMessage = "Username must be at least 4 characters long")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Enter your password")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[a-zA-Z])[\u0020-\u007E]{0,}$",
            ErrorMessage = "Password must contain at least one latin letter, one digit and no cyrillic symbols.")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Enter your email")]
        [EmailAddress(ErrorMessage = "Please enter valid email address")]
        public string? Email { get; set; }
    }
}
