using TripsServiceBLL.DTO.Drivers;

namespace TripsServiceBLL.Interfaces
{
	public interface IDriverService
	{
		IQueryable<ReadDriverDTO> GetDriversOverall();
	}
}
