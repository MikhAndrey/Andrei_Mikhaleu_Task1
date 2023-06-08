using TripsServiceBLL.Services;
using TripsServiceDAL.Entities;
using TripsServiceBLL.Infrastructure;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.DTO.Trips;

namespace TripsServiceBLL.Commands.Trips
{
    public class GetOthersPublicTripsCommand : IAsyncCommand<List<TripDTO>>
    {
        private TripService _tripService;

        private UserService _userService;

        private string _userName;

        public GetOthersPublicTripsCommand(TripService tripService, UserService userService, string userName)
        {
            _tripService = tripService;
            _userService = userService;
            _userName = userName;
        }

        public async Task<List<TripDTO>> ExecuteAsync()
        {
            User? user = await _userService.GetByUserNameAsync(_userName);
            return _tripService.GetOthersPublicTrips(user.UserId);
        }
    }
}
