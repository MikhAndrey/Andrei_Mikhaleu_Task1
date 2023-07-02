using AutoMapper;
using TripsServiceBLL.DTO.Drivers;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Infrastructure.Mappers;

public class DriverMapper : Profile
{
    public DriverMapper()
    {
        CreateMap<Driver, ReadDriverDTO>()
            .ForMember(dest => dest.FirstPhoto, opt => opt.MapFrom(src => src.Photos.FirstOrDefault()));
        CreateMap<Driver, DriverDetailsDTO>()
            .ForMember(dest => dest.Feedbacks, opt => opt.MapFrom(src => src.Trips.Where(t => t.Feedback != null)));
    }
}