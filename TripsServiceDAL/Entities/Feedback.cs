using TripsServiceDAL.Interfaces;

namespace TripsServiceDAL.Entities
{
    public class Feedback : IIdentifiable
    {
        public int Id { get; set; }

        public Trip? Trip { get; set; }

        public int TripId { get; set; }

        public string? Text { get; set; }

        public int Rating { get; set; }
    }
}
