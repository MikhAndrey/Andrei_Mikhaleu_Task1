using Microsoft.AspNetCore.Http;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Interfaces
{
    public interface IImageService
    {
        Task UploadImagesAsync(Trip trip, List<IFormFile> images, string webRootPath);

        Task DeleteByIdAsync(int imageId, int tripId, int userId, string webRootPath);

        void DeleteTripImages(Trip trip, string webRootPath);

        void CreateImagesDirectory(string webRootPath);
    }
}
