using TripsServiceBLL.DTO.Comments;

namespace TripsServiceBLL.Interfaces
{
    public interface ICommentService
    {
        Task AddCommentAsync(CreateCommentDTO comment, int userId);

        Task DeleteCommentAsync(int commentId);

        Task DeleteByTripIdAsync(int tripId);
    }
}
