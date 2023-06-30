using AutoMapper;
using TripsServiceBLL.DTO.Comments;
using TripsServiceBLL.Interfaces;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Infrastructure.Exceptions;
using TripsServiceDAL.Interfaces;

namespace TripsServiceBLL.Services
{
	public class CommentService : ICommentService
	{
		private readonly IUnitOfWork _unitOfWork;

		private readonly IMapper _mapper;

		public CommentService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task AddCommentAsync(CreateCommentDTO comment)
		{
			_unitOfWork.Trips.ThrowErrorIfNotExists(comment.TripId);
			
			Comment commentToAdd = _mapper.Map<Comment>(comment);
			await _unitOfWork.Comments.AddAsync(commentToAdd);	
			await _unitOfWork.SaveAsync();
		}

		public async Task DeleteCommentAsync(int commentId)
		{
			Comment? commentToDelete = await _unitOfWork.Comments.GetByIdAsync(commentId);
			if (commentToDelete == null)
			{
				throw new EntityNotFoundException(TripsServiceDAL.Utils.UtilConstants.GetEntityNotExistsMessage<Comment>()());
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
