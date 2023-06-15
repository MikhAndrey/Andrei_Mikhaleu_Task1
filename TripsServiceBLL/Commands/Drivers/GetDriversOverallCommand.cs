using TripsServiceBLL.DTO.Drivers;
using TripsServiceBLL.Interfaces;

namespace TripsServiceBLL.Commands.Drivers
{
	public class GetDriversOverallCommand : ICommand<IEnumerable<ReadDriverDTO>>
	{
		private readonly IDriverService _driverService;

		public GetDriversOverallCommand(IDriverService driverService)
		{
			_driverService = driverService;
		}

		public IEnumerable<ReadDriverDTO> Execute()
		{
			return _driverService.GetDriversOverall();
		}
	}
}
