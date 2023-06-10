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
			List<string> newImageFileNames = new();
			foreach (var image in images)
			{
				if (image != null && image.Length > 0)
				{
					string? extension = Path.GetExtension(image.FileName);
					string newFileName = $"{Guid.NewGuid()}{extension}";
					string filePath = Path.Combine(webRootPath, Constants.ImagesFolderName, newFileName);
					using (var fileStream = new FileStream(filePath, FileMode.Create))
					{
						await image.CopyToAsync(fileStream);
					}
					newImageFileNames.Add($"/{Constants.ImagesFolderName}/{newFileName}");
				}
			}
			foreach (string imageUrl in newImageFileNames)
			{
				trip.Images.Add(new Image { Link = imageUrl });
			}
		}

		public async Task DeleteByIdAsync(int imageId, string webRootPath)
		{
			Image? image = await _unitOfWork.Images.GetByIdAsync(imageId);

			if (image != null)
			{
				_unitOfWork.Images.Delete(image);
				await _unitOfWork.SaveAsync();

				string path = webRootPath + image.Link;

				if (File.Exists(path))
					File.Delete(path);
			}
		}

		public void DeleteTripImages(Trip trip)
		{
			foreach (Image image in trip.Images)
				if (File.Exists(image.Link))
					File.Delete(image.Link);
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
