using Microsoft.EntityFrameworkCore;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Infrastructure;
using TripsServiceDAL.Interfaces;

namespace TripsServiceDAL.Repos;

public class DriverRepository : EFGenericRepository<Driver>, IDriverRepository
{
	public DriverRepository(TripsDBContext context) : base(context)
	{
	}

	public new IQueryable<Driver> GetAll()
	{
		return _dbSet.Include(d => d.Photos).Include(d => d.Trips).ThenInclude(t => t.Feedback);
	}

	public new async Task<Driver> GetByIdAsync(int id)
	{
		Driver? driver = await _dbSet
			.Include(d => d.Photos)
			.Include(d => d.Trips)
			.ThenInclude(t => t.Feedback)
			.Include(d => d.Trips)
			.ThenInclude(t => t.User)
			.FirstOrDefaultAsync(d => d.Id == id);
		ThrowErrorIfEntityIsNull(driver);
		return driver;
	}
}