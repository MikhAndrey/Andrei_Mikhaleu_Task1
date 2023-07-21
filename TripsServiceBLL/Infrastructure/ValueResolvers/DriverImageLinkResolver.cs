using AutoMapper;
using TripsServiceBLL.DTO.Images;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Utils;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Infrastructure.ValueResolvers;

public class DriverImageLinkResolver : IValueResolver<Driver, IHasImages, List<ImageDTO>>
{
    public List<ImageDTO> Resolve(Driver driver, IHasImages dto, List<ImageDTO> images, ResolutionContext context)
    {
        return driver.Images.Select(image => new ImageDTO()
        {
            Id = image.Id,
            Link = $"{UtilConstants.ImagesFolderName}/{UtilConstants.DriversFolderName}/{driver.Id}/{image.Link}"
        }).ToList();
    }
}