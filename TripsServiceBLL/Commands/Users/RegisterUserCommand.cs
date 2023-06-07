using TripsServiceBLL.DTO;
using TripsServiceBLL.Services;
using TripsServiceDAL.Entities;
using TripsServiceBLL.Infrastructure;

namespace TripsServiceBLL.Commands.Users
{
    public class RegisterUserCommand : IAsyncCommand
    {
        private readonly UserService _userService;

        private readonly UserSignupDTO _user;

        public RegisterUserCommand(UserService userService, UserSignupDTO user)
        {
            _userService = userService;
            _user = user;
        }

        public async Task ExecuteAsync()
        {
            User? existingUser = await _userService.GetByUserNameAsync(_user.UserName);
            if (existingUser != null)
                throw new ValidationException("This username is already taken", "");
            User newUser = new()
            {
                UserName = _user.UserName,
                Password = _user.Password
            };

            await _userService.AddAsync(newUser);
        }
    }
}
