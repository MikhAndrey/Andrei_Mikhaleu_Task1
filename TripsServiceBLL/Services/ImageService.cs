﻿using Microsoft.AspNetCore.Http;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Utils;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Interfaces;

namespace TripsServiceBLL.Services;

public class ImageService : IImageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;

    public ImageService(IUnitOfWork unitOfWork, IUserService userService)
    {
        _unitOfWork = unitOfWork;
        _userService = userService;
    }

    public List<string> GenerateImagesFileNames(List<IFormFile>? images)
    {
        List<string> fileNames = new();

        if (images != null)
        {
            foreach (IFormFile image in images)
            {
                if (image != null && image.Length > 0)
                {
                    string? extension = Path.GetExtension(image.FileName);
                    string newFileName = $"{Guid.NewGuid()}{extension}";
                    fileNames.Add(newFileName);
                }
            }
        }

        return fileNames;
    }

    public async Task AddTripImagesAsync(List<string> fileNames, int tripId)
    {
        foreach (string fileName in fileNames)
        {
            await _unitOfWork.Images.AddAsync(new Image
            {
                TripId = tripId,
                Link = fileName
            });
        }

        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteByTripIdAsync(int tripId)
    {
        IQueryable<Image> imagesToDelete = _unitOfWork.Images.GetByTripId(tripId);
        foreach (Image image in imagesToDelete)
        {
            _unitOfWork.Images.Delete(image);
        }

        await _unitOfWork.SaveAsync();
    }

    public async Task SaveTripImagesFilesAsync(int tripId, int userId, List<string> fileNames, List<IFormFile>? images,
        string                                     webRootPath)
    {
        if (images != null)
        {
            int imagesCount = images.Count;
            for (int i = 0; i < imagesCount; i++)
            {
                if (images[i] != null && images[i].Length > 0)
                {
                    string userFilePath = Path.Combine(webRootPath, UtilConstants.ImagesFolderName, userId.ToString());
                    if (!Directory.Exists(userFilePath))
                    {
                        Directory.CreateDirectory(userFilePath);
                    }

                    string tripFilePath = Path.Combine(userFilePath, tripId.ToString());
                    if (!Directory.Exists(tripFilePath))
                    {
                        Directory.CreateDirectory(tripFilePath);
                    }

                    string filePath = Path.Combine(tripFilePath, fileNames[i]);

                    using FileStream fileStream = new(filePath, FileMode.Create);
                    await images[i].CopyToAsync(fileStream);
                }
            }
        }
    }

    public async Task DeleteByIdAsync(int imageId, int tripId, string webRootPath)
    {
        Image image = await _unitOfWork.Images.GetByIdAsync(imageId);

        _unitOfWork.Trips.ThrowErrorIfNotExists(tripId);

        _unitOfWork.Images.Delete(image);
        await _unitOfWork.SaveAsync();
        _ = _userService.GetCurrentUserId();
        /*string path = Path.Combine(webRootPath, Utils.UtilConstants.ImagesFolderName, userId.ToString(), tripId.ToString(), image.Link);

        if (File.Exists(path))
        {
            File.Delete(path);
        }*/ //TODO: What to do with physical images is under discussion
    }

    public void DeleteTripImagesFiles(int tripId, int userId, string webRootPath)
    {
        string tripDirectoryPath = Path.Combine(webRootPath, UtilConstants.ImagesFolderName, userId.ToString(),
            tripId.ToString());
        if (Directory.Exists(tripDirectoryPath))
        {
            Directory.Delete(tripDirectoryPath, true);
        }
    }

    public void CreateImagesDirectory(string webRootPath)
    {
        string path = Path.Combine(webRootPath, UtilConstants.ImagesFolderName);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
}