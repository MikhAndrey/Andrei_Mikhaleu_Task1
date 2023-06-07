namespace TripsServiceBLL.DTO
{
    public class UserDTO
    {
        public UserDTO()
        {
            Trips = new();
            Comments = new();
            UserName = "";
            Password = "";
        }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public List<TripDTO> Trips { get; set; }

        public List<CommentDTO> Comments { get; set; }
    }
}
