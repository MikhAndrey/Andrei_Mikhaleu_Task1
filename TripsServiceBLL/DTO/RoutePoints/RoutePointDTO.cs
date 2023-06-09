using TripsServiceBLL.DTO.Trips;

namespace TripsServiceBLL.DTO.RoutePoints
{
    public class RoutePointDTO
    {
        /*public RoutePointDTO()
        {
            Trip = new();
        }*/
        public int RoutePointId { get; set; }

        public decimal Longitude { get; set; }

        public decimal Latitude { get; set; }

        public int Ordinal { get; set; }

        public int TripId { get; set; }

        public ReadTripDTO Trip { get; set; }
    }
}
