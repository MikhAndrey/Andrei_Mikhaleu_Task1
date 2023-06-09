﻿using Microsoft.EntityFrameworkCore;
using TripsServiceDAL.Infrastructure;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Interfaces;

namespace TripsServiceDAL.Repos
{
    public class TripRepository : EFGenericRepository<Trip>, ITripRepository
    {
        public TripRepository(TripsDBContext context) : base(context) { }

        public async new Task<Trip?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(t => t.RoutePoints)
                .Include(t => t.Images)
                .Include(t => t.Comments)
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.TripId == id);
        }

        public IQueryable<int> GetYearsOfUserTrips(int userId)
        {
            return _dbSet.Include(t => t.User).Where(t => t.UserId == userId)
                .Select(t => t.StartTime.Year).Distinct().OrderByDescending(el => el);
        }

        public IQueryable<Trip> GetUserTripsWithUser(int userId)
        {
            return _dbSet.Include(t => t.User).Where(t => t.UserId == userId);
        }

        public IQueryable<Trip> GetTripsByUserId(int userId)
        {
            return _dbSet.Where(t => t.UserId == userId);
        }

        public IQueryable<Trip> GetOthersPublicTrips(int userId)
        {
            return _dbSet
                .Include(t => t.User)
                .Where(t => t.UserId != userId && t.Public);
        }

        public IQueryable<Trip> GetHistoryOfTripsByUserId(int userId)
        {
			return _dbSet.Where(t => t.UserId == userId && t.EndTime < DateTime.UtcNow);
		}
    }
}
