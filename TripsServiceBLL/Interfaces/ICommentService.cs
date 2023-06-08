using TripsServiceBLL.DTO.Comments;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Interfaces
{
    public interface ICommentService
    {
        Task AddCommentAsync(CommentDTO comment, User user);

        Task DeleteCommentAsync(int commentId);
    }
}
