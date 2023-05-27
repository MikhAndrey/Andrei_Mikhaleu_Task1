using Andrei_Mikhaleu_Task1.Models.Entities;
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

        public void Add(RoutePoint routePoint)
        {
            _context.RoutePoints.Add(routePoint);
            _context.SaveChanges();
        }

        public void Update(RoutePoint routePoint)
        {
            _context.RoutePoints.Update(routePoint);
            _context.SaveChanges();
        }

        public void Delete(RoutePoint routePoint)
        {
            _context.RoutePoints.Remove(routePoint);
            _context.SaveChanges();
        }

        public RoutePoint GetById(int id)
        {
            return _context.RoutePoints
                .Include(rp => rp.Trip)
                .FirstOrDefault(rp => rp.RoutePointId == id);
        }

        public List<RoutePoint> GetRoutePointsByYear(int year) 
        {
            return _context.RoutePoints.Where(x => x.Trip.StartTime.Year == year)
                .Include(rp => rp.Trip.User).ToList();
        }

        public List<RoutePoint> GetRoutePointsByTripId(int tripId)
        {
            return _context.RoutePoints
                .Include(rp => rp.Trip)
                .Where(rp => rp.TripId == tripId)
                .ToList();
        }
    }
}
