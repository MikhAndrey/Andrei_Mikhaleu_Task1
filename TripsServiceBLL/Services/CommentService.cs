using TripsServiceDAL.Entities;
using TripsServiceDAL.Infrastructure;
using TripsServiceBLL.DTO.Comments;
using TripsServiceDAL.Interfaces;
using TripsServiceBLL.Interfaces;

namespace TripsServiceBLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddCommentAsync(CommentDTO comment, User user)
        {
            Trip? trip = await _unitOfWork.Trips.GetByIdAsync(comment.TripId);

            if (trip != null)
            {
                trip.Comments.Add(new Comment
                {
                    Message = comment.Message,
                    TripId = comment.TripId,
                    Date = DateTime.UtcNow,
                    User = user
                });

                _unitOfWork.Trips.Update(trip);
                await _unitOfWork.SaveAsync();
            }
        }

        public async Task DeleteCommentAsync(int commentId)
        {
            Comment? commentToDelete = await _unitOfWork.Comments.GetByIdAsync(commentId);
            if (commentToDelete != null)
            {
                _unitOfWork.Comments.Delete(commentToDelete);
                await _unitOfWork.SaveAsync();
            }
        }
    }
}
