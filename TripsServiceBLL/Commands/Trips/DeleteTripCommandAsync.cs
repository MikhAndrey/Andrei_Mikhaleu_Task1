using TripsServiceBLL.Infrastructure.Exceptions;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Utils;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Commands.Trips
{
    public class DeleteTripCommandAsync : ICommandAsync
    {
        private readonly int _id;

        private readonly ITripService _tripService;

        private readonly IImageService _imageService;

        private readonly string _webRootPath;

        public DeleteTripCommandAsync(int id, ITripService tripService, IImageService imageService, string webRootPath)
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
                throw new EntityNotFoundException(Constants.GetEntityNotExistsMessage("trip"));
            }

            _imageService.DeleteTripImages(tripToDelete, _webRootPath);
            await _tripService.DeleteAsync(tripToDelete);
        }
    }
}
