using Microsoft.EntityFrameworkCore;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Infrastructure;
using TripsServiceDAL.Interfaces;

namespace TripsServiceDAL.Repos
{
	public class DriverRepository : EFGenericRepository<Driver>, IDriverRepository
	{
		public DriverRepository(TripsDBContext context) : base(context) { }

		public IQueryable<Driver> GetAllWithRating()
		{
			return _dbSet.Include(d => d.Photos).Select(d => new Driver{
				Id = d.Id,
				Name = d.Name,
				Experience = d.Experience,
				Photos = d.Photos,
				AverageRating = d.Trips.Where(t => t.Feedback != null).Select(t => (double?)t.Feedback.Rating).Average() ?? 0
			});
		}
	}
}
