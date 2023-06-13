using TripsServiceBLL.DTO.Images;
using TripsServiceBLL.DTO.RoutePoints;

namespace TripsServiceBLL.DTO.Trips
{
    public class EditTripDTO : CreateTripDTO
    {
        public List<ImageDTO> Images { get; set; }

        public List<RoutePointDTO> RoutePoints { get; set; }

        public int TripId { get; set; }

        public int UserId { get; set; }

        public EditTripDTO() { }
    }
}
