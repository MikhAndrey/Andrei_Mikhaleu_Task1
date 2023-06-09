using TripsServiceBLL.DTO.Comments;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Interfaces
{
	public interface ICommentService
	{
		Task AddCommentAsync(CreateCommentDTO comment, Trip trip, User user);

		Task DeleteCommentAsync(int commentId);
	}
}
