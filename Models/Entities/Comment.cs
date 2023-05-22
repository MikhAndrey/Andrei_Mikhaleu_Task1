namespace Andrei_Mikhaleu_Task1.Models.Entities
{
    public class Comment
    {
        public int CommentId { get; set; }

        public string Message { get; set; }

        public DateTime Date { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public int TripId { get; set; }

        public Trip Trip { get; set; }

    }
}
