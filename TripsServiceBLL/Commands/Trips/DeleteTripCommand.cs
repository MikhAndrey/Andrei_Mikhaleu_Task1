using TripsServiceBLL.Infrastructure;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Utils;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Commands.Trips
{
    public class DeleteTripCommand : IAsyncCommand
    {
        private readonly int _id;

        private readonly ITripService _tripService;

        private readonly IImageService _imageService;

        private readonly string _webRootPath;

        public DeleteTripCommand(int id, ITripService tripService, IImageService imageService, string webRootPath)
        {
            _id = id;
            _tripService = tripService;
            _imageService = imageService;
            _webRootPath = webRootPath;
        }

        public async Task ExecuteAsync()
        {
            Trip? tripToDelete = await _tripService.GetByIdWithImagesAsync(_id);
            if (tripToDelete == null)
            {
                throw new EntityNotFoundException(Constants.TripNotExistsMessage);
            }

            _imageService.DeleteTripImages(tripToDelete, _webRootPath);
            await _tripService.DeleteAsync(tripToDelete);
        }
    }
}
