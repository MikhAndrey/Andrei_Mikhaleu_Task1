using TripsServiceDAL.Interfaces;

namespace TripsServiceDAL.Entities
{
    public class Comment : IIdentifiable
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public DateTime Date { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public int TripId { get; set; }

        public Trip Trip { get; set; }

    }
}
