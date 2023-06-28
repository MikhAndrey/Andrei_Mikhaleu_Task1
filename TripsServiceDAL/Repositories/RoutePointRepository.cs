using TripsServiceDAL.Entities;
using TripsServiceDAL.Infrastructure;
using TripsServiceDAL.Interfaces;

namespace TripsServiceDAL.Repos
{
	public class RoutePointRepository : EFGenericRepository<RoutePoint>, IRoutePointRepository
	{
		public RoutePointRepository(TripsDBContext context) : base(context) { }

		public IQueryable<RoutePoint> GetRoutePointsByYear(int year, int userId)
		{
			return _dbSet
				.Where(x => (x.Trip.StartTime.Year == year || x.Trip.EndTime.Year == year) && x.Trip.UserId == userId);
		}

		public IQueryable<RoutePoint> GetRoutePointsByTripId(int tripId)
		{
			return _dbSet.Where(rp => rp.TripId == tripId);
		}
	}
}
