using Microsoft.AspNetCore.Http;

namespace TripsServiceBLL.Interfaces;

public interface IImageService
{
	Task SaveTripImagesAsync(int tripId, List<IFormFile?>? images);
	Task DeleteByIdAsync(int imageId, int tripId);
	Task DeleteByTripIdAsync(int tripId);
	void CreateImagesDirectory();
}
