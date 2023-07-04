using System.ComponentModel.DataAnnotations;

namespace TripsServiceBLL.DTO.Users;

public class UserLoginDTO
{
	[Required(ErrorMessage = "Enter your login")]
	public string UserName { get; set; }

	[Required(ErrorMessage = "Enter your password")]
	public string Password { get; set; }

	[Display(Name = "Remember me")] public bool RememberMe { get; set; }
}
