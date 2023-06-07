namespace TripsServiceBLL.DTO
{
    public class TripDTO : ExtendedExistingTripDTO
    {
        public List<CommentDTO> Comments { get; set; }

        public UserDTO User { get; set; }

        public int UserId { get; set; }
    }
}
