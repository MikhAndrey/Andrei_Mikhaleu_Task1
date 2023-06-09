using TripsServiceBLL.Interfaces;
using TripsServiceBLL.DTO.Users;

namespace TripsServiceBLL.Commands.Users
{
	public class RegisterUserCommand : IAsyncCommand
	{
		private readonly IUserService _userService;

		private readonly UserSignupDTO _user;

		public RegisterUserCommand(IUserService userService, UserSignupDTO user)
		{
			_userService = userService;
			_user = user;
		}

		public async Task ExecuteAsync()
		{
			await _userService.TryToRegisterNewUserAsync(_user);
		}
	}
}
