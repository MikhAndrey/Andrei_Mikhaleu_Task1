using TripsServiceBLL.DTO.RoutePoints;

namespace TripsServiceBLL.Interfaces
{
    public interface IRoutePointService
    {
        Task ParseAndAddRoutePoints(int tripId, string routePoints);

        IQueryable<RoutePointCoordinatesDTO> GetRoutePointsByYear(int year, int userId);

        void DeleteByTripId(int tripId);
    }
}
