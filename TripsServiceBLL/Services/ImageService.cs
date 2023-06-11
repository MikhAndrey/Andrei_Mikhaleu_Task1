using TripsServiceDAL.Entities;
using Microsoft.AspNetCore.Http;
using TripsServiceDAL.Interfaces;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Utils;

namespace TripsServiceBLL.Services
{
	public class ImageService : IImageService
	{
		private readonly IUnitOfWork _unitOfWork;

		public ImageService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task UploadImagesAsync(Trip trip, List<IFormFile> images, string webRootPath)
		{
			foreach (var image in images)
			{
				if (image != null && image.Length > 0)
				{
					string? extension = Path.GetExtension(image.FileName);
					string newFileName = $"{Guid.NewGuid()}{extension}";
					string userFilePath = Path.Combine(webRootPath, Constants.ImagesFolderName, trip.User.UserId.ToString());
					if (!Directory.Exists(userFilePath))
						Directory.CreateDirectory(userFilePath);
					string tripFilePath = Path.Combine(userFilePath, trip.TripId.ToString());
                    if (!Directory.Exists(tripFilePath))
                        Directory.CreateDirectory(tripFilePath);
                    string filePath = Path.Combine(tripFilePath, newFileName);
					using (var fileStream = new FileStream(filePath, FileMode.Create))
					{
						await image.CopyToAsync(fileStream);
					}
                    trip.Images.Add(new Image { Link = newFileName });
                }
			}
		}

		public async Task DeleteByIdAsync(int imageId, int tripId, int userId, string webRootPath)
		{
			Image? image = await _unitOfWork.Images.GetByIdAsync(imageId);

			if (image != null)
			{
				_unitOfWork.Images.Delete(image);
				await _unitOfWork.SaveAsync();

				string path = Path.Combine(webRootPath, Constants.ImagesFolderName, userId.ToString(), tripId.ToString(), image.Link);

				if (File.Exists(path))
					File.Delete(path);
			}
		}

		public void DeleteTripImages(Trip trip, string webRootPath)
		{
			foreach (Image image in trip.Images)
			{
				string path = Path.Combine(webRootPath, Constants.ImagesFolderName, trip.User.UserId.ToString(), trip.TripId.ToString(), image.Link);
				if (File.Exists(path))
					File.Delete(path);
			}
			string tripDirectoryPath = Path.Combine(webRootPath, Constants.ImagesFolderName, trip.User.UserId.ToString(), trip.TripId.ToString());
			if (Directory.Exists(tripDirectoryPath))
				Directory.Delete(tripDirectoryPath);
        }

		public void CreateImagesDirectory(string webRootPath)
		{
			string path = Path.Combine(webRootPath, Constants.ImagesFolderName);

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
		}
	}
}
