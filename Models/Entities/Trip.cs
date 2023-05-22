namespace Andrei_Mikhaleu_Task1.Models.Entities
{
    public class Trip
    {
        public int TripId { get; set; }

        public string Name { get; set; }

        public virtual List<RoutePoint> RoutePoints { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public decimal Distance { get; set; }

        public string Description { get; set; }

        public bool Completed { get; set; }

        public bool Public { get; set; }

        public List<Image> Images { get; set; }

        public List<Comment> Comments { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
    }
}
