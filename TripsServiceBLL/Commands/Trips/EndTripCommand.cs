using TripsServiceBLL.Interfaces;
using TripsServiceDAL.Entities;

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
            Trip? trip = await _tripService.GetByIdAsync(_id);
            if (trip != null && trip.StartTime < DateTime.UtcNow && trip.EndTime > DateTime.UtcNow)
            {
                _tripService.SetNewTimeForEndingTrip(trip);
                await _tripService.UpdateAsync(trip);
            }
        }
    }
}
