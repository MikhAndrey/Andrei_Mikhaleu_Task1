using AutoMapper;
using TripsServiceBLL.DTO.Images;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Infrastructure.Mappers;

public class ImageMapper : Profile
{
	public ImageMapper()
	{
		CreateMap<Image, ImageDTO>();
		CreateMap<DriverPhoto, ImageDTO>();
	}
}
