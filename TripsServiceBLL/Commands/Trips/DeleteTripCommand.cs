using TripsServiceDAL.Entities;
using TripsServiceBLL.Interfaces;

namespace TripsServiceBLL.Commands.Trips
{
    public class DeleteTripCommand : IAsyncCommand
    {
        private readonly int _id;

        private readonly ITripService _tripService;

        private readonly IImageService _imageService;

        public DeleteTripCommand(int id, ITripService tripService, IImageService imageService)
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
