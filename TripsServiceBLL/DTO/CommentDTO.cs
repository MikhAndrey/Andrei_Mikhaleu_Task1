namespace TripsServiceBLL.DTO
{
    public class CommentDTO
    {
        public CommentDTO()
        {
            Trip = new();
            User = new();
        }

        public int CommentId { get; set; }

        public string Message { get; set; }

        public DateTime Date { get; set; }

        public int UserId { get; set; }

        public UserDTO User { get; set; }

        public int TripId { get; set; }

        public TripDTO Trip { get; set; }
    }
}
