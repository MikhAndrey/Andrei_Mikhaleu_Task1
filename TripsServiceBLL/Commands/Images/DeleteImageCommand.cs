using TripsServiceBLL.Interfaces;

namespace TripsServiceBLL.Commands.Images
{
	public class DeleteImageCommand : IAsyncCommand
	{
		private readonly string _webRootPath;

		private readonly int _imageId;

        private readonly int _tripId;

        private readonly int _userId;

        private readonly IImageService _imageService;

		public DeleteImageCommand(string webRootPath, int imageId, int tripId, int userId, IImageService imageService)
		{
			_webRootPath = webRootPath;
			_imageId = imageId;
			_tripId = tripId;
			_userId = userId;
			_imageService = imageService;
		}

		public async Task ExecuteAsync()
		{
			await _imageService.DeleteByIdAsync(_imageId, _tripId, _userId, _webRootPath);
		}
	}
}
