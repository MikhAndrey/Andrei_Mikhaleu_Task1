using TripsServiceBLL.DTO.Trips;

namespace TripsServiceBLL.DTO.Images
{
    public class ImageDTO
    {
        public int ImageId { get; set; }

        public string? Link { get; set; }

        public int TripId { get; set; }

        public ReadTripDTO? Trip { get; set; }
    }
}
