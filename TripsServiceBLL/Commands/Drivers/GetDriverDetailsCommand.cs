using TripsServiceBLL.DTO.Drivers;
using TripsServiceBLL.Interfaces;

namespace TripsServiceBLL.Commands.Drivers
{
    public class GetDriverDetailsCommand : IAsyncCommand<DriverDetailsDTO>
    {
        private readonly IDriverService _driverService;

        private readonly int _id;

        public GetDriverDetailsCommand(IDriverService driverService, int id)
        {
            _driverService = driverService;
            _id = id;
        }

        public async Task<DriverDetailsDTO> ExecuteAsync()
        {
            return await _driverService.GetDriverDetailsAsync(_id);
        }
    }
}
