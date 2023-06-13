using TripsServiceBLL.DTO.RoutePoints;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Interfaces
{
    public interface IRoutePointService
    {
        void ParseAndAddRoutePoints(Trip trip, string routePoints);

        IQueryable<RoutePointCoordinatesDTO> GetRoutePointsByYear(int year, int userId);
    }
}
