using AutoMapper;
using TripsServiceBLL.DTO.Comments;
using TripsServiceBLL.Interfaces;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Interfaces;

namespace TripsServiceBLL.Services;

public class CommentService : ICommentService
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;

	private readonly IUserService _userService;

	public CommentService(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_userService = userService;
	}

	public async Task<CommentDTO> AddCommentAsync(CreateCommentDTO comment)
	{
		_unitOfWork.Trips.ThrowErrorIfNotExists(comment.TripId);

		Comment commentToAdd = _mapper.Map<Comment>(comment);
		await _unitOfWork.Comments.AddAsync(commentToAdd);
		await _unitOfWork.SaveAsync();

		CommentDTO result =  _mapper.Map<CommentDTO>(commentToAdd);
		result.UserName = _userService.GetCurrentUserName();
		return result;
	}

	public async Task DeleteCommentAsync(int commentId)
	{
		Comment commentToDelete = await _unitOfWork.Comments.GetByIdAsync(commentId);

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
