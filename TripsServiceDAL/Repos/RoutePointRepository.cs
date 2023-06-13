using Microsoft.EntityFrameworkCore;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Infrastructure;
using TripsServiceDAL.Interfaces;

namespace TripsServiceDAL.Repos
{
    public class RoutePointRepository : EFGenericRepository<RoutePoint>, IRoutePointRepository
    {
        public RoutePointRepository(TripsDBContext context) : base(context) { }

        public new async Task<RoutePoint?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(rp => rp.Trip)
                .FirstOrDefaultAsync(rp => rp.RoutePointId == id);
        }

        public IQueryable<RoutePoint> GetRoutePointsByYear(int year, int userId)
        {
            return _dbSet
                .Where(x => x.Trip.StartTime.Year == year || x.Trip.EndTime.Year == year)
                .Include(rp => rp.Trip.User)
                .Where(rp => rp.Trip.UserId == userId);
        }

        public IQueryable<RoutePoint> GetRoutePointsByTripId(int tripId)
        {
            return _dbSet
                .Include(rp => rp.Trip)
                .Where(rp => rp.TripId == tripId);
        }
    }
}
