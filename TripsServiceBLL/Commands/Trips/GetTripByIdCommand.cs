using TripsServiceBLL.DTO.Trips;
using TripsServiceBLL.Interfaces;

namespace TripsServiceBLL.Commands.Trips
{
    public class GetTripByIdCommand : IAsyncCommand<EditTripDTO>
    {
        private ITripService _tripService;

        private int _id;

        public GetTripByIdCommand(ITripService tripService, int id)
        {
            _tripService = tripService;
            _id = id;
        }

        public async Task<EditTripDTO> ExecuteAsync()
        {
            return await _tripService.GetTripForEditingAsync(_id);
        }
    }
}
