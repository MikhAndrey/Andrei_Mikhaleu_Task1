using TripsServiceDAL.Entities;
using TripsServiceDAL.Infrastructure;
using Microsoft.AspNetCore.Http;
using TripsServiceBLL.Infrastructure;

namespace TripsServiceBLL.Services
{
    public class ImageService
    {
        private readonly UnitOfWork _unitOfWork;

        public ImageService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task UploadImages(Trip trip, List<IFormFile> images, string webRootPath)
        {
            List<string> newImageFileNames = new();
            foreach (var image in images)
            {
                if (image != null && image.Length > 0)
                {
                    string? extension = Path.GetExtension(image.FileName);
                    string newFileName = $"{Guid.NewGuid()}{extension}";
                    string filePath = Path.Combine(webRootPath, "images", newFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }
                    newImageFileNames.Add($"/images/{newFileName}");
                }
            }
            foreach (string imageUrl in newImageFileNames)
            {
                trip.Images.Add(new Image { Link = imageUrl });
            }
        }

        public async Task DeleteById(int imageId, string webRootPath)
        {
            Image? image = await _unitOfWork.Images.GetByIdAsync(imageId);

            if (image == null)
            {
                throw new ValidationException("Image was not found", "");
            }

            _unitOfWork.Images.Delete(image);
            await _unitOfWork.Save();

            string path = webRootPath + image.Link;

            if (File.Exists(path))
                File.Delete(path);
            else
                throw new ValidationException("File of image was not found", "");
        }

        public void DeleteTripImages(Trip trip)
        {
            foreach (Image image in trip.Images)
                if (File.Exists(image.Link))
                    File.Delete(image.Link);
        }

        public void CreateImagesDirectory(string webRootPath)
        {
            string path = Path.Combine(webRootPath, "images");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
