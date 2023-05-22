namespace Andrei_Mikhaleu_Task1.Models.Entities
{
    public class RoutePoint
    {
        public int RoutePointId { get; set; }

        public decimal Longitude { get; set; }

        public decimal Latitude { get; set; }

        public int Ordinal { get; set; }

        public int TripId { get; set; }

        public Trip Trip { get; set; }
    }
}
