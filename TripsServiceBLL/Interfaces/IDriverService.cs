using TripsServiceBLL.DTO.Drivers;

namespace TripsServiceBLL.Interfaces
{
    public interface IDriverService
    {
        IEnumerable<ReadDriverDTO> GetDriversOverall();

        Task<DriverDetailsDTO> GetDriverDetailsAsync(int id);
    }
}
