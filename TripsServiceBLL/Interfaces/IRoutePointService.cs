using TripsServiceBLL.DTO.RoutePoints;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Interfaces;

public interface IRoutePointService
{
	Task AddTripRoutePointsAsync(int tripId, string routePointsAsString);
	IEnumerable<RoutePointCoordinatesDTO> GetRoutePointsByYear(int year);
	Task DeleteByTripIdAsync(int tripId);
}
