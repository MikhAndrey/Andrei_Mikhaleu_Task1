using Microsoft.AspNetCore.Http;

namespace TripsServiceBLL.Interfaces
{
    public interface IImageService
    {
        List<string> GenerateImagesFileNames(List<IFormFile>? images);

        Task AddTripImagesAsync(List<string> fileNames, int tripId);

        Task SaveTripImagesFilesAsync(int tripId, int userId, List<string> fileNames, List<IFormFile>? images, string webRootPath);

        Task DeleteByIdAsync(int imageId, int tripId, int userId, string webRootPath);

        void DeleteTripImagesFiles(int tripId, int userId, string webRootPath);

        Task DeleteByTripIdAsync(int tripId);

        void CreateImagesDirectory(string webRootPath);
    }
}
