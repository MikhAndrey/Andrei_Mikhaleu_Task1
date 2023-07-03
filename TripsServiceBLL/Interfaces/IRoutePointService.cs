using TripsServiceBLL.DTO.RoutePoints;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Interfaces;

public interface IRoutePointService
{
	List<RoutePoint>? ParseRoutePointsFromString(string routePoints);
	Task AddTripRoutePointsAsync(int tripId, List<RoutePoint>? routePoints);
	IQueryable<RoutePointCoordinatesDTO> GetRoutePointsByYear(int year);
	Task DeleteByTripIdAsync(int tripId);
}