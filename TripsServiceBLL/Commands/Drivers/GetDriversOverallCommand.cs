using TripsServiceBLL.DTO.Drivers;
using TripsServiceBLL.Interfaces;

namespace TripsServiceBLL.Commands.Drivers
{
    public class GetDriversOverallCommand : ICommand<IQueryable<ReadDriverDTO>>
    {
        private readonly IDriverService _driverService;

        public GetDriversOverallCommand(IDriverService driverService)
        {
            _driverService = driverService;
        }

        public IQueryable<ReadDriverDTO> Execute()
        {
            return _driverService.GetDriversOverall();
        }
    }
}
