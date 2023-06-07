using TripsServiceBLL.DTO;
using TripsServiceBLL.Services;
using TripsServiceBLL.Infrastructure;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Commands.Trips
{
    public class GetUserTripsCommand : IAsyncGenericCommand<List<TripDTO>>
    {
        private TripService _tripService;

        private UserService _userService;

        private string _userName;

        public GetUserTripsCommand(TripService tripService, UserService userService, string userName)
        {
            _tripService = tripService;
            _userService = userService;
            _userName = userName;
        }

        public async Task<List<TripDTO>> ExecuteAsync()
        {
            User? user = await _userService.GetByUserNameAsync(_userName);
            if (user == null)
                throw new ValidationException("User was not found", "");
            List<TripDTO> trips = _tripService.GetTripsByUserId(user.UserId);
            return trips;
        }
    }
}
