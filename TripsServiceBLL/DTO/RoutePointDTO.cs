namespace TripsServiceBLL.DTO
{
    public class RoutePointDTO
    {
        public RoutePointDTO()
        {
            Trip = new();
        }
        public int RoutePointId { get; set; }

        public decimal Longitude { get; set; }

        public decimal Latitude { get; set; }

        public int Ordinal { get; set; }

        public int TripId { get; set; }

        public TripDTO Trip { get; set; }
    }
}
