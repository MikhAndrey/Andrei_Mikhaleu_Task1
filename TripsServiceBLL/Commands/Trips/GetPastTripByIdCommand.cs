using TripsServiceBLL.DTO.Trips;
using TripsServiceBLL.Interfaces;

namespace TripsServiceBLL.Commands.Trips
{
    public class GetPastTripByIdCommand : IAsyncCommand<EditPastTripDTO>
    {
        private readonly ITripService _tripService;

        private readonly int _id;

        public GetPastTripByIdCommand(ITripService tripService, int id)
        {
            _tripService = tripService;
            _id = id;
        }

        public async Task<EditPastTripDTO> ExecuteAsync()
        {
            return await _tripService.GetPastTripForEditingAsync(_id);
        }
    }
}
