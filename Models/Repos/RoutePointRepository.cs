using Andrei_Mikhaleu_Task1.Common;
using Andrei_Mikhaleu_Task1.Models.Entities;
using Andrei_Mikhaleu_Task1.Models.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace Andrei_Mikhaleu_Task1.Models.Repos
{
    public class RoutePointRepository
    {
        private readonly TripsDBContext _context;

        public RoutePointRepository(TripsDBContext context)
        {
            _context = context;
        }

        public async Task Add(RoutePoint routePoint)
        {
            await _context.RoutePoints.AddAsync(routePoint);
            await _context.SaveChangesAsync();
        }

        public async Task Update(RoutePoint routePoint)
        {
            _context.RoutePoints.Update(routePoint);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(RoutePoint routePoint)
        {
            _context.RoutePoints.Remove(routePoint);
            await _context.SaveChangesAsync();
        }

        public async Task<RoutePoint> GetById(int id)
        {
            return await _context.RoutePoints
                .Include(rp => rp.Trip)
                .FirstOrDefaultAsync(rp => rp.RoutePointId == id);
        }

        public async Task<List<RoutePoint>> GetRoutePointsByYear(int year, int userId) 
        {
            return await _context.RoutePoints.Where(x => x.Trip.StartTime.Year == year)
                .Include(rp => rp.Trip.User).Where(rp => rp.Trip.UserId == userId).ToListAsync();
        }

        public async Task<List<RoutePoint>> GetRoutePointsByTripId(int tripId)
        {
            return await _context.RoutePoints
                .Include(rp => rp.Trip)
                .Where(rp => rp.TripId == tripId)
                .ToListAsync();
        }
    }
}
