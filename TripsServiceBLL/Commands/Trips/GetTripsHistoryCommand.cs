using TripsServiceBLL.DTO;
using TripsServiceBLL.Services;
using TripsServiceDAL.Entities;
using TripsServiceBLL.Infrastructure;

namespace TripsServiceBLL.Commands.Trips
{
    public class GetTripsHistoryCommand : IAsyncGenericCommand<List<TripDTO>>
    {
        private TripService _tripService;

        private UserService _userService;

        private string _userName;

        public GetTripsHistoryCommand(TripService tripService, UserService userService, string userName)
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
            List<TripDTO> trips = _tripService.GetHistoryOfTripsByUserId(user.UserId);
            return trips;
        }
    }
}
