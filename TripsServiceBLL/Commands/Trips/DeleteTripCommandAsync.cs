using Microsoft.AspNetCore.Hosting;
using TripsServiceBLL.Interfaces;

namespace TripsServiceBLL.Commands.Trips
{
    public class DeleteTripCommandAsync : ICommandAsync<int>
    {

        private readonly ITripService _tripService;

        private readonly IImageService _imageService;

        private readonly IWebHostEnvironment _env;

        public DeleteTripCommandAsync(
            ITripService tripService, 
            IImageService imageService, 
            IWebHostEnvironment env)
        {
            _tripService = tripService;
            _imageService = imageService;
            _env = env;
        }

        public async Task ExecuteAsync(int id)
        {
            await _imageService.DeleteTripImages(id, _env.WebRootPath);
            await _tripService.DeleteAsync(id);
        }
    }
}
