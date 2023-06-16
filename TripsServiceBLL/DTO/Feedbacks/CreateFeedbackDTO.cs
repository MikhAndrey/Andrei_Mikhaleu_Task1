namespace TripsServiceBLL.DTO.Feedbacks
{
	public class CreateFeedbackDTO
	{
		public int TripId { get; set; }
		
		public string? Text { get; set; }

		public int Rating { get; set; }
	}
}
