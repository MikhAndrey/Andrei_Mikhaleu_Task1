namespace TripsServiceBLL.DTO
{
    public class ExtendedExistingTripDTO : ExistingTripDTO
    {
        public List<ImageDTO> Images { get; set; }

        public List<RoutePointDTO> RoutePoints { get; set; }
    }
}
