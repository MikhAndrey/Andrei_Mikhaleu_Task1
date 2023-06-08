using TripsServiceBLL.DTO.Trips;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Services;

namespace TripsServiceBLL.Commands.Trips
{
    public class GetTripByIdCommand : IAsyncGenericCommand<ExtendedExistingTripDTO>
    {
        private TripService _tripService;

        private int _id;

        public GetTripByIdCommand(TripService tripService, int id)
        {
            _tripService = tripService;
            _id = id;
        }

        public async Task<ExtendedExistingTripDTO> ExecuteAsync()
        {
            return await _tripService.InitializeExtendedExistingTripAsync(_id);
        }
    }
}
