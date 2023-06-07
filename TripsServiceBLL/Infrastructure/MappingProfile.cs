using AutoMapper;
using TripsServiceBLL.DTO;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Infrastructure
{
    public class MappingProfile : Profile
    { 
        public MappingProfile() { 
            CreateMap<Comment, CommentDTO>(); 
            CreateMap<Image, ImageDTO>(); 
            CreateMap<RoutePoint, RoutePointDTO>(); 
            CreateMap<Trip, TripDTO>();
            CreateMap<Trip, ExtendedExistingTripDTO>();
            CreateMap<User, UserDTO>();
            CreateMap<List<TripDTO>,IQueryable<Trip>>();
            CreateMap<ExtendedExistingTripDTO, Trip>();
        } 
    }
}
