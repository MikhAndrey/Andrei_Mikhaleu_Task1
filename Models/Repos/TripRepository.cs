using Andrei_Mikhaleu_Task1.Common;
using Andrei_Mikhaleu_Task1.Models.Entities;
using Andrei_Mikhaleu_Task1.Models.Entities.Common;
using Andrei_Mikhaleu_Task1.Models.Entities.Special;
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

        public async Task Add(Trip trip)
        {
            await _context.Trips.AddAsync(trip);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Trip trip)
        {
            _context.Trips.Update(trip);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Trip trip)
        {
            _context.Trips.Remove(trip);
            await _context.SaveChangesAsync();
        }

        public async Task<Trip> GetById(int id)
        {
            return await _context.Trips
                .Include(t => t.RoutePoints)
                .Include(t => t.Images)
                .Include(t => t.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(t => t.TripId == id);
        }

        public async Task<List<Trip>> GetAllTripsWithUsers(int userId)
        {
            return await _context.Trips.Include(t => t.User)
                .Where(t => t.UserId == userId).ToListAsync();
        }

        public async Task<List<DurationInMonth>> GetTotalDurationByMonths(int year, int userId)
        {
            List<Trip> trips = await GetAllTripsWithUsers(userId);

            List<DurationInMonth> result = Enumerable.Range(1, 12)
                .Select(month => new DurationInMonth()
                {
                    Month = new DateTime(year, month, 1).ToString("MMMM"),
                    TotalDuration = trips
                        .Where(t => t.StartTime.Year == year && t.StartTime.Month <= month && t.EndTime.Year == year && t.EndTime.Month >= month)
                        .Select(t =>
                        {
                            var start = t.StartTime <= new DateTime(year, month, 1) ? new DateTime(year, month, 1) : t.StartTime;
                            var end = t.EndTime >= new DateTime(year, month, DateTime.DaysInMonth(year, month)) ? new DateTime(year, month, DateTime.DaysInMonth(year, month)) : t.EndTime;
                            return (end - start).TotalHours;
                        })
                    .DefaultIfEmpty(0)
                    .Sum()
                }).ToList();
			return result;
        }

        public async Task<List<Trip>> GetTripsByUserId(int userId)
        {
            return await _context.Trips.Where(t => t.UserId == userId).ToListAsync();
        }

        public async Task<List<Trip>> GetOthersPublicTrips(int userId)
        {
            return await _context.Trips.Where(t => t.UserId != userId && t.Public)
                .Include(t => t.User).ToListAsync();
        }
    }
}
