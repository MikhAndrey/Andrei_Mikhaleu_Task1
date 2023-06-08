using TripsServiceBLL.Services;
using TripsServiceDAL.Entities;
using TripsServiceBLL.Infrastructure;
using TripsServiceBLL.Interfaces;

namespace TripsServiceBLL.Commands.Trips
{
    public class StartTripCommand : IAsyncCommand
    {
        private readonly TripService _tripService;

        private readonly int _id;

        public StartTripCommand(TripService tripService, int id)
        {
            _tripService = tripService;
            _id = id;
        }

        public async Task ExecuteAsync()
        {
            Trip? trip = await _tripService.GetByIdAsync(_id);
            if (trip == null)
                throw new ValidationException("Trip not found", "");
            if (trip.StartTime > DateTime.UtcNow)
            {
                _tripService.SetNewTimeForStartingTrip(trip);
                await _tripService.UpdateAsync(trip);
            }
        }
    }
}
