using AutoMapper;
using TripsServiceBLL.DTO.Images;
using TripsServiceBLL.DTO.Trips;
using TripsServiceBLL.Utils;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Infrastructure.ValueResolvers;

public class ImageLinkResolver : IValueResolver<Trip, TripDetailsDTO, List<ImageDTO>>
{
    public List<ImageDTO> Resolve(Trip trip, TripDetailsDTO dto, List<ImageDTO> images, ResolutionContext context)
    {
        return trip.Images.Select(image => new ImageDTO()
        {
            Id = image.Id,
            Link = $"{UtilConstants.ImagesFolderName}/{trip.UserId.ToString()}/{trip.Id.ToString()}/{image.Link}"
        }).ToList();
    }
}