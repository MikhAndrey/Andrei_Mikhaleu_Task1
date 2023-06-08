using TripsServiceBLL.Infrastructure;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Services;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Commands.Trips
{
    public class EndTripCommand : IAsyncCommand
    {
        private readonly TripService _tripService;

        private readonly int _id;

        public EndTripCommand(TripService tripService, int id)
        {
            _tripService = tripService;
            _id = id;
        }

        public async Task ExecuteAsync()
        {
            Trip? trip = await _tripService.GetByIdAsync(_id);
            if (trip == null)
                throw new ValidationException("Trip not found", "");
            if (trip.StartTime < DateTime.UtcNow && trip.EndTime > DateTime.UtcNow)
            {
                _tripService.SetNewTimeForEndingTrip(trip);
                await _tripService.UpdateAsync(trip);
            }
        }
    }
}
