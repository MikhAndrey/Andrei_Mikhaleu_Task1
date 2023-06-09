using TripsServiceBLL.Interfaces;
using TripsServiceBLL.DTO.Trips;

namespace TripsServiceBLL.Commands.Trips
{
    public class GetTripsHistoryCommand : IAsyncCommand<List<TripDTO>>
    {
        private ITripService _tripService;

        private int _userId;

        public GetTripsHistoryCommand(ITripService tripService, int userId)
        {
            _tripService = tripService;
            _userId = userId;
        }

        public async Task<List<TripDTO>> ExecuteAsync()
        {
            return _tripService.GetHistoryOfTripsByUserId(_userId);
        }
    }
}
