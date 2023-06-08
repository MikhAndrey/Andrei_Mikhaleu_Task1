using TripsServiceBLL.DTO.Images;
using TripsServiceBLL.DTO.RoutePoints;

namespace TripsServiceBLL.DTO.Trips
{
    public class ExtendedExistingTripDTO : ExistingTripDTO
    {
        public List<ImageDTO> Images { get; set; }

        public List<RoutePointDTO> RoutePoints { get; set; }
    }
}
