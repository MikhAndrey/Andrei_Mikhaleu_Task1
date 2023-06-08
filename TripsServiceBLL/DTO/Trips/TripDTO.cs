using TripsServiceBLL.DTO.Comments;
using TripsServiceBLL.DTO.Users;

namespace TripsServiceBLL.DTO.Trips
{
    public class TripDTO : ExtendedExistingTripDTO
    {
        public List<CommentDTO> Comments { get; set; }

        public UserDTO User { get; set; }

        public int UserId { get; set; }
    }
}
