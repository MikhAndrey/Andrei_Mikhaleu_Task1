using TripsServiceBLL.DTO.Trips;
using TripsServiceBLL.Interfaces;

namespace TripsServiceBLL.Commands.Trips
{
    public class GetTripDetailsCommand : IAsyncCommand<TripDetailsDTO>
    {
        private readonly ITripService _tripService;

        private readonly int _id;

        private readonly int _userId;

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
