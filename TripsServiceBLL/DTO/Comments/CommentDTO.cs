namespace TripsServiceBLL.DTO.Comments;

public class CommentDTO : CreateCommentDTO
{
	public int Id { get; set; }
	public DateTime Date { get; set; }
	public string UserName { get; set; }
	public string TimeAgoAsString { get; set; }
}
