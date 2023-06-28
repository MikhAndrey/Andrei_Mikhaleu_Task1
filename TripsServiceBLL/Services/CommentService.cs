using TripsServiceBLL.DTO.Comments;
using TripsServiceBLL.Infrastructure.Exceptions;
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
				throw new EntityNotFoundException(UtilConstants.GetEntityNotFoundMessage("trip"));
			}

			bool userExists = _unitOfWork.Users.Exists(userId);
			if (!userExists)
			{
				throw new EntityNotFoundException(UtilConstants.GetEntityNotFoundMessage("user"));
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
				throw new EntityNotFoundException(UtilConstants.GetEntityNotExistsMessage("comment"));
			}

			_unitOfWork.Comments.Delete(commentToDelete);
			await _unitOfWork.SaveAsync();
		}

		public async Task DeleteByTripIdAsync(int tripId)
		{
			IQueryable<Comment> commentsToDelete = _unitOfWork.Comments.GetByTripId(tripId);
			foreach (Comment comment in commentsToDelete)
			{
				_unitOfWork.Comments.Delete(comment);
			}
			await _unitOfWork.SaveAsync();
		}
	}
}
