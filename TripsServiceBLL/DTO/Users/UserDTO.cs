using TripsServiceBLL.DTO.Comments;
using TripsServiceBLL.DTO.Trips;

namespace TripsServiceBLL.DTO.Users
{
    public class UserDTO
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public List<ReadTripDTO> Trips { get; set; }

        public List<CommentDTO> Comments { get; set; }
    }
}
