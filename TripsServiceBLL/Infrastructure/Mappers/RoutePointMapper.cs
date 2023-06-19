using AutoMapper;
using TripsServiceBLL.DTO.RoutePoints;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Infrastructure.Mappers
{
    public class RoutePointMapper : Profile
    {
        public RoutePointMapper()
        {
            CreateMap<RoutePoint, RoutePointDTO>();
        }
    }
}
