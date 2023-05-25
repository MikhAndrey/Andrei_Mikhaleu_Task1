using Andrei_Mikhaleu_Task1.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Andrei_Mikhaleu_Task1.Models.Repos
{
    public class TripRepository
    {
        private readonly TripsDBContext _context;

        public TripRepository(TripsDBContext context)
        {
            _context = context;
        }

        public void Add(Trip trip)
        {
            _context.Trips.Add(trip);
            _context.SaveChanges();
        }

        public void Update(Trip trip)
        {
            _context.Trips.Update(trip);
            _context.SaveChanges();
        }

        public void Delete(Trip trip)
        {
            _context.Trips.Remove(trip);
            _context.SaveChanges();
        }

        public Trip GetById(int id)
        {
            return _context.Trips
                .Include(t => t.RoutePoints)
                .Include(t => t.Images)
                .Include(t => t.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefault(t => t.TripId == id);
        }

        public List<Trip> GetTripsByUserId(int userId)
        {
            return _context.Trips.Where(t => t.UserId == userId).ToList();
        }

        public List<Trip> GetOthersPublicTrips(int userId)
        {
            return _context.Trips.Where(t => t.UserId != userId && t.Public)
                .Include(t => t.User).ToList();
        }
    }
}
