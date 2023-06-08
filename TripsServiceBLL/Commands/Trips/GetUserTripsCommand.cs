using TripsServiceDAL.Entities;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.DTO.Trips;

namespace TripsServiceBLL.Commands.Trips
{
    public class GetUserTripsCommand : IAsyncCommand<List<TripDTO>>
    {
        private ITripService _tripService;

        private IUserService _userService;

        private string _userName;

        public GetUserTripsCommand(ITripService tripService, IUserService userService, string userName)
        {
            _tripService = tripService;
            _userService = userService;
            _userName = userName;
        }

        public async Task<List<TripDTO>> ExecuteAsync()
        {
            User? user = await _userService.GetByUserNameAsync(_userName);
            return _tripService.GetTripsByUserId(user.UserId);
        }
    }
}
