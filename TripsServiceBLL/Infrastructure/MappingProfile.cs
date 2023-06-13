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
			_ = CreateMap<Comment, CommentDTO>();
			_ = CreateMap<Image, ImageDTO>();
			_ = CreateMap<RoutePoint, RoutePointDTO>();
			_ = CreateMap<Trip, ReadTripDTO>();
			_ = CreateMap<User, UserDTO>();
			_ = CreateMap<List<ReadTripDTO>, IQueryable<Trip>>();
			_ = CreateMap<CreateTripDTO, Trip>();
		}
	}
}
