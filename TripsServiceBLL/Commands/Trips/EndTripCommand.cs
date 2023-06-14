using TripsServiceBLL.Interfaces;

namespace TripsServiceBLL.Commands.Trips
{
    public class EndTripCommand : IAsyncCommand
    {
        private readonly ITripService _tripService;

        private readonly int _id;

        public EndTripCommand(ITripService tripService, int id)
        {
            _tripService = tripService;
            _id = id;
        }

        public async Task ExecuteAsync()
        {
            await _tripService.EndTripAsync(_id);
        }
    }
}
