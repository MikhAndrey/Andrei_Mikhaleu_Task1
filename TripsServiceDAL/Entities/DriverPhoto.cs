using TripsServiceDAL.Interfaces;

namespace TripsServiceDAL.Entities
{
    public class DriverPhoto : IIdentifiable, ISoftDelete
    {
        public int Id { get; set; }

        public string? Link { get; set; }

        public int DriverId { get; set; }

        public Driver? Driver { get; set; }

        public bool IsDeleted { get; set; }
    }
}
