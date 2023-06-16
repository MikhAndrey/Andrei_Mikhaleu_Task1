using TripsServiceBLL.DTO.Comments;
using TripsServiceBLL.Infrastructure;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Utils;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Interfaces;

namespace TripsServiceBLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddCommentAsync(CreateCommentDTO comment, int userId)
        {
            bool tripExists = _unitOfWork.Trips.Exists(comment.TripId);
            if (!tripExists)
            {
                throw new EntityNotFoundException(Constants.TripNotFoundMessage);
            }

            bool userExists = _unitOfWork.Users.Exists(userId);
            if (!userExists)
            {
                throw new EntityNotFoundException(Constants.UserNotFoundMessage);
            }

            await _unitOfWork.Comments.AddAsync(new Comment
            {
                Message = comment.Message,
                TripId = comment.TripId,
                Date = DateTime.UtcNow,
                UserId = userId
            });

            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteCommentAsync(int commentId)
        {
            Comment? commentToDelete = await _unitOfWork.Comments.GetByIdAsync(commentId);
            if (commentToDelete == null)
            {
                throw new EntityNotFoundException(Constants.CommentNotExistsMessage);
            }

            _unitOfWork.Comments.Delete(commentToDelete);
            await _unitOfWork.SaveAsync();
        }
    }
}
