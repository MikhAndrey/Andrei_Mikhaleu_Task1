using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Interfaces
{
	public interface IDriverService
	{
		IQueryable<Driver> GetAllWithRating();
	}
}
