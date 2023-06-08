using TripsServiceDAL.Entities;

namespace TripsServiceBLL.DTO.Trips
{
    public class ExistingTripDTO : NewTripDTO
    {
        public int TripId { get; set; }

        public bool IsCurrentUserTrip { get; set; }
    }
}
