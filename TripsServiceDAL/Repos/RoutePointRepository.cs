﻿using Microsoft.EntityFrameworkCore;
using TripsServiceDAL.Infrastructure;
using TripsServiceDAL.Entities;

namespace TripsServiceDAL.Repos
{
    public class RoutePointRepository : EFGenericRepository<RoutePoint>
    {
        public RoutePointRepository(TripsDBContext context) : base(context) { }

        public async new Task<RoutePoint?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(rp => rp.Trip)
                .FirstOrDefaultAsync(rp => rp.RoutePointId == id);
        }

        public IQueryable<RoutePoint> GetRoutePointsByYear(int year, int userId)
        {
            return _dbSet
                .Where(x => x.Trip.StartTime.Year == year)
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