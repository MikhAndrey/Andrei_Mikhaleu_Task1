using Microsoft.EntityFrameworkCore;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Infrastructure;
using TripsServiceDAL.Interfaces;

namespace TripsServiceDAL.Repos
{
    public class TripRepository : EFGenericRepository<Trip>, ITripRepository
    {
        public TripRepository(TripsDBContext context) : base(context) { }

        public async Task<Trip?> GetByIdForDetailsAsync(int id)
        {
            return await _dbSet
                .Include(t => t.RoutePoints)
                .Include(t => t.Images)
                .Include(t => t.Comments)
                    .ThenInclude(c => c.User)
                .Include(t => t.User)
                .Include(t => t.Feedback)
                .Include(t => t.Driver)
                    .ThenInclude(d => d.Photos)
                .Include(t => t.Driver)
                    .ThenInclude(d => d.Trips)
                    .ThenInclude(t => t.Feedback)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Trip?> GetByIdForEditingAsync(int id)
        {
            return await _dbSet
                .Include(t => t.RoutePoints)
                .Include(t => t.Images)
                .Include(t => t.User)
                .Include(t => t.Driver)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Trip?> GetByIdForDeleteAsync(int id)
        {
            return await _dbSet
                .Include(t => t.RoutePoints)
                .Include(t => t.Images)
                .Include(t => t.Comments)
                .Include(t => t.Feedback)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Trip?> GetByIdForMinimalEditingAsync(int id)
        {
            return await _dbSet
                .Include(t => t.Images)
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Trip?> GetByIdWithImagesAsync(int id)
        {
            return await _dbSet
                .Include(t => t.Images)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Trip?> GetByIdWithImagesAndRoutePointsAsync(int id)
        {
            return await _dbSet
                .Include(t => t.Images)
                .Include(t => t.RoutePoints)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public IQueryable<int> GetYearsOfUserTrips(int userId)
        {
            return _dbSet.Include(t => t.User).Where(t => t.UserId == userId)
                .Select(t => t.StartTime.Year).Distinct().OrderByDescending(el => el);
        }

        public IQueryable<Trip> GetUserTripsWithUser(int userId)
        {
            return _dbSet.Where(t => t.UserId == userId).Include(t => t.User);
        }

        public IQueryable<Trip> GetTripsByUserId(int userId)
        {
            return _dbSet.Where(t => t.UserId == userId).Include(t => t.Feedback);
        }

        public IQueryable<Trip> GetTripsByYearAndUserId(int year, int userId)
        {
            return _dbSet.Where(t => t.UserId == userId && (t.StartTime.Year == year || t.EndTime.Year == year));
        }

        public IQueryable<Trip> GetOthersPublicTrips(int userId)
        {
            return _dbSet
                .Include(t => t.User)
                .Where(t => t.UserId != userId && t.Public);
        }

        public IQueryable<Trip> GetHistoryOfTripsByUserId(int userId)
        {
            return _dbSet.Where(t => t.UserId == userId && t.EndTime < DateTime.UtcNow).Include(t => t.Feedback);
        }
    }
}
