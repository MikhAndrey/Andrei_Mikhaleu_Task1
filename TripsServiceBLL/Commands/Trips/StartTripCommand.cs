using TripsServiceBLL.Interfaces;

namespace TripsServiceBLL.Commands.Trips
{
    public class StartTripCommand : IAsyncCommand
    {
        private readonly ITripService _tripService;

        private readonly int _id;

        public StartTripCommand(ITripService tripService, int id)
        {
            _tripService = tripService;
            _id = id;
        }

        public async Task ExecuteAsync()
        {
            await _tripService.StartTripAsync(_id);
        }
    }
}
