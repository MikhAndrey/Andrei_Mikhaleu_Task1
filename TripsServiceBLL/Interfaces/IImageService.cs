using Microsoft.AspNetCore.Http;

namespace TripsServiceBLL.Interfaces
{
    public interface IImageService
    {
        Task UploadImagesAsync(int tripId, int userId, List<IFormFile>? images, string webRootPath);

        Task DeleteByIdAsync(int imageId, int tripId, int userId, string webRootPath);

        Task DeleteTripImages(int tripId, string webRootPath);

        void CreateImagesDirectory(string webRootPath);
    }
}
