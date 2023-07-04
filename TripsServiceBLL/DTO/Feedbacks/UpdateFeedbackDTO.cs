namespace TripsServiceBLL.DTO.Feedbacks;

public class UpdateFeedbackDTO
{
	public int Id { get; set; }

	public string? Text { get; set; }

	public int Rating { get; set; }
}