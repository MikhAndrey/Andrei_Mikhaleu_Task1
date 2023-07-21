using AutoMapper;
using TripsServiceBLL.DTO.Drivers;
using TripsServiceBLL.Infrastructure.ValueResolvers;
using TripsServiceBLL.Utils;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Infrastructure.Mappers;

public class DriverMapper : Profile
{
	public DriverMapper(DriverImageLinkResolver driverImageLinkResolver)
	{
		CreateMap<Driver, ReadDriverDTO>()
			.ForMember(dest => dest.PhotoLink, opt => opt.MapFrom(src =>
				src.Images.Any() ?
				$"{UtilConstants.ImagesFolderName}/{UtilConstants.DriversFolderName}/{src.Id}/{src.Images.FirstOrDefault().Link}" :
				null
			));
		CreateMap<Driver, DriverDetailsDTO>()
			.ForMember(dest => dest.Feedbacks, opt => opt.MapFrom(src => src.Trips.Where(t => t.Feedback != null)))
			.ForMember(dest => dest.Images, opt => opt.MapFrom(driverImageLinkResolver));
	}
}
