using TripsServiceBLL.DTO.Comments;

namespace TripsServiceBLL.Interfaces;

public interface ICommentService
{
	Task<CommentDTO> AddCommentAsync(CreateCommentDTO comment);
	Task DeleteCommentAsync(int commentId);
	Task DeleteByTripIdAsync(int tripId);
}
