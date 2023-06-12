using TripsServiceBLL.Interfaces;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Interfaces;

namespace TripsServiceBLL.Services
{
	public class DriverService : IDriverService
	{
		private readonly IUnitOfWork _unitOfWork;

		public DriverService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IQueryable<Driver> GetAllWithRating()
		{
			return _unitOfWork.Drivers.GetAllWithRating();
		}
	}
}
