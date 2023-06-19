using TripsServiceBLL.Interfaces;

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
            await _imageService.DeleteTripImages(_id, _webRootPath);
            await _tripService.DeleteAsync(_id);
        }
    }
}
