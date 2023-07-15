using System.ComponentModel.DataAnnotations;

namespace TripsServiceBLL.DTO.Comments;

public class CreateCommentDTO
{
	public int TripId { get; set; }

	[Required(ErrorMessage = "Please, enter text of your comment")]
	public string? Message { get; set; }
}
