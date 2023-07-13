using AutoMapper;
using TripsServiceBLL.DTO.Drivers;
using TripsServiceBLL.Utils;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Infrastructure.Mappers;

public class DriverMapper : Profile
{
	public DriverMapper()
	{
		CreateMap<Driver, ReadDriverDTO>()
			.ForMember(dest => dest.PhotoLink, opt => opt.MapFrom(src =>
				src.Photos.Any() ?
				$"{UtilConstants.ImagesFolderName}/{UtilConstants.DriversFolderName}/{src.Id}/{src.Photos.FirstOrDefault().Link}" :
				null
			));
		CreateMap<Driver, DriverDetailsDTO>()
			.ForMember(dest => dest.Feedbacks, opt => opt.MapFrom(src => src.Trips.Where(t => t.Feedback != null)));
	}
}
