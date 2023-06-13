using TripsServiceBLL.DTO.Users;
using TripsServiceBLL.Interfaces;

namespace TripsServiceBLL.Commands.Users
{
    public class GetLoginJWTTokenCommand : IAsyncCommand<string>
    {
        private readonly IUserService _userService;

        private readonly UserLoginDTO _user;

        public GetLoginJWTTokenCommand(IUserService userService, UserLoginDTO user)
        {
            _userService = userService;
            _user = user;
        }

        public async Task<string> ExecuteAsync()
        {
            return await _userService.GetJWTTokenAsync(_user);
        }
    }
}
