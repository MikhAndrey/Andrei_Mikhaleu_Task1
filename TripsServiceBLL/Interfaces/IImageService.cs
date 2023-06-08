using Microsoft.AspNetCore.Http;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Interfaces
{
    public interface IImageService
    {
        Task UploadImagesAsync(Trip trip, List<IFormFile> images, string webRootPath);

        Task DeleteByIdAsync(int imageId, string webRootPath);

        void DeleteTripImages(Trip trip);

        void CreateImagesDirectory(string webRootPath);
    }
}
