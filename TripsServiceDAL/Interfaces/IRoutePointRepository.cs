using TripsServiceDAL.Entities;

namespace TripsServiceDAL.Interfaces
{
	public interface IRoutePointRepository : IGenericRepository<RoutePoint>
	{
		IQueryable<RoutePoint> GetRoutePointsByYear(int year, int userId);

		IQueryable<RoutePoint> GetRoutePointsByTripId(int tripId);
	}
}
