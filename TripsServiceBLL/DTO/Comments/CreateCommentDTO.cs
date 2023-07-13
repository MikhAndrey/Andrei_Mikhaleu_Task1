using System.ComponentModel.DataAnnotations;

namespace TripsServiceBLL.DTO.Comments;

public class CreateCommentDTO
{
	public int TripId { get; set; }

	[Required(ErrorMessage = "Comment message can't be empty")]
	public string? Message { get; set; }
}
