namespace TripsServiceBLL.DTO.Feedbacks
{
    public class ReadFeedbackDTO
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public int Rating { get; set; }
        public string UserName { get; set; }
    }
}