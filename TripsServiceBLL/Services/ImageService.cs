using Microsoft.AspNetCore.Http;
using TripsServiceBLL.Infrastructure.Exceptions;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Utils;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Interfaces;

namespace TripsServiceBLL.Services
{
    public class ImageService : IImageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ImageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task UploadImagesAsync(int tripId, int userId, List<IFormFile>? images, string webRootPath)
        {
            if (images != null)
            {
                foreach (IFormFile image in images)
                {
                    if (image != null && image.Length > 0)
                    {
                        string? extension = Path.GetExtension(image.FileName);
                        string newFileName = $"{Guid.NewGuid()}{extension}";
                        string userFilePath = Path.Combine(webRootPath, Constants.ImagesFolderName, userId.ToString());
                        if (!Directory.Exists(userFilePath))
                        {
                            Directory.CreateDirectory(userFilePath);
                        }

                        string tripFilePath = Path.Combine(userFilePath, tripId.ToString());
                        if (!Directory.Exists(tripFilePath))
                        {
                            Directory.CreateDirectory(tripFilePath);
                        }

                        string filePath = Path.Combine(tripFilePath, newFileName);
                        using (FileStream fileStream = new(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(fileStream);
                        }
                        await _unitOfWork.Images.AddAsync(new Image
                        {
                            TripId = tripId,
                            Link = newFileName
                        });
                    }
                }
                await _unitOfWork.SaveAsync();
            }
        }

        public async Task DeleteByIdAsync(int imageId, int tripId, int userId, string webRootPath)
        {
            Image? image = await _unitOfWork.Images.GetByIdAsync(imageId);
            if (image == null)
            {
                throw new EntityNotFoundException(Constants.GetEntityNotExistsMessage("message"));
            }

            bool tripExists = _unitOfWork.Trips.Exists(tripId);
            if (!tripExists)
            {
                throw new EntityNotFoundException(Constants.GetEntityNotFoundMessage("trip"));
            }

            bool userExists = _unitOfWork.Users.Exists(userId);
            if (!userExists)
            {
                throw new EntityNotFoundException(Constants.GetEntityNotFoundMessage("user"));
            }

            _unitOfWork.Images.Delete(image);
            await _unitOfWork.SaveAsync();

            string path = Path.Combine(webRootPath, Constants.ImagesFolderName, userId.ToString(), tripId.ToString(), image.Link);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public async Task DeleteTripImages(int tripId, string webRootPath)
        {
            Trip? trip = await _unitOfWork.Trips.GetByIdWithImagesAsync(tripId);
            if (trip == null)
            {
                throw new EntityNotFoundException(Constants.GetEntityNotExistsMessage("trip"));
            }

            foreach (Image image in trip.Images)
            {
                string path = Path.Combine(webRootPath, Constants.ImagesFolderName, trip.UserId.ToString(), trip.Id.ToString(), image.Link);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            string tripDirectoryPath = Path.Combine(webRootPath, Constants.ImagesFolderName, trip.UserId.ToString(), trip.Id.ToString());
            if (Directory.Exists(tripDirectoryPath))
            {
                Directory.Delete(tripDirectoryPath);
            }
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
