using TripsServiceBLL.Interfaces;
using TripsServiceBLL.DTO.Trips;

namespace TripsServiceBLL.Commands.Trips
{
    public class GetTripDetailsCommand : IAsyncCommand<TripDTO>
    {
        private ITripService _tripService;

        private int _id;

        private int _userId;

        public GetTripDetailsCommand(ITripService tripService, int id, int userId)
        {
            _tripService = tripService;
            _id = id;
            _userId = userId;
        }

        public async Task<TripDTO> ExecuteAsync()
        {
            return await _tripService.InitializeTripDTOAsync(_id, _userId);
        }
    }
}
