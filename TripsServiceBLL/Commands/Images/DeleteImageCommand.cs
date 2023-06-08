using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Services;

namespace TripsServiceBLL.Commands.Images
{
    public class DeleteImageCommand : IAsyncCommand
    {
        private readonly string _webRootPath;

        private readonly int _imageId;

        private readonly ImageService _imageService;

        public DeleteImageCommand(string webRootPath, int imageId, ImageService imageService)
        {
            _webRootPath = webRootPath;
            _imageId = imageId;
            _imageService = imageService;
        }

        public async Task ExecuteAsync()
        {
            await _imageService.DeleteByIdAsync(_imageId, _webRootPath);
        }
    }
}
