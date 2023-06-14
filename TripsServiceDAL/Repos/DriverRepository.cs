using Microsoft.EntityFrameworkCore;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Infrastructure;
using TripsServiceDAL.Interfaces;

namespace TripsServiceDAL.Repos
{
    public class DriverRepository : EFGenericRepository<Driver>, IDriverRepository
    {
        public DriverRepository(TripsDBContext context) : base(context) { }

        public IQueryable<Driver> GetAll()
        {
            return _dbSet.Include(d => d.Photos).Include(d => d.Trips).ThenInclude(t => t.Feedback);
        }
    }
}
