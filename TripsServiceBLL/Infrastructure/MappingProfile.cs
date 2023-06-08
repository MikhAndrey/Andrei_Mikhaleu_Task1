using AutoMapper;
using TripsServiceBLL.DTO.Comments;
using TripsServiceBLL.DTO.Images;
using TripsServiceBLL.DTO.RoutePoints;
using TripsServiceBLL.DTO.Trips;
using TripsServiceBLL.DTO.Users;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Comment, CommentDTO>();
            CreateMap<Image, ImageDTO>();
            CreateMap<RoutePoint, RoutePointDTO>();
            CreateMap<Trip, TripDTO>();
            CreateMap<Trip, ExtendedExistingTripDTO>();
            CreateMap<User, UserDTO>();
            CreateMap<List<TripDTO>, IQueryable<Trip>>();
            CreateMap<ExtendedExistingTripDTO, Trip>();
            CreateMap<NewTripDTO, Trip>();
        }
    }
}
