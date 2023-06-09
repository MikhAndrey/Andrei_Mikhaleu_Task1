using TripsServiceBLL.Interfaces;
using TripsServiceBLL.DTO.Trips;

namespace TripsServiceBLL.Commands.Trips
{
    public class GetUserTripsCommand : IAsyncCommand<List<TripDTO>>
    {
        private ITripService _tripService;

        private int _userId;

        public GetUserTripsCommand(ITripService tripService, int userId)
        {
            _tripService = tripService;
            _userId = userId;
        }

        public async Task<List<TripDTO>> ExecuteAsync()
        {
            return _tripService.GetTripsByUserId(_userId);
        }
    }
}
