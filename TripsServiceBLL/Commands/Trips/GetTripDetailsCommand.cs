using TripsServiceBLL.Interfaces;
using TripsServiceBLL.DTO.Trips;

namespace TripsServiceBLL.Commands.Trips
{
    public class GetTripDetailsCommand : IAsyncCommand<TripDetailsDTO>
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

        public async Task<TripDetailsDTO> ExecuteAsync()
        {
            return await _tripService.GetTripDetailsAsync(_id, _userId);
        }
    }
}
