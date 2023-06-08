using TripsServiceBLL.Services;
using TripsServiceBLL.Infrastructure;
using TripsServiceDAL.Entities;
using TripsServiceBLL.Interfaces;

namespace TripsServiceBLL.Commands.Trips
{
    public class DeleteTripCommand : IAsyncCommand
    {
        private readonly int _id;

        private readonly TripService _tripService;

        private readonly ImageService _imageService;

        public DeleteTripCommand(int id, TripService tripService, ImageService imageService)
        {
            _id = id;
            _tripService = tripService;
            _imageService = imageService;
        }

        public async Task ExecuteAsync()
        {
            Trip? tripToDelete = await _tripService.GetByIdAsync(_id);
            if (tripToDelete != null)
            {
                _imageService.DeleteTripImages(tripToDelete);
                await _tripService.DeleteAsync(tripToDelete);
            }
        }
    }
}
