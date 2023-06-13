using TripsServiceBLL.DTO.Comments;
using TripsServiceBLL.Interfaces;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Commands.Comments
{
	public class AddCommentCommand : IAsyncCommand
	{
		private readonly ICommentService _commentService;

		private readonly IUserService _userService;

		private readonly ITripService _tripService;

		private readonly CreateCommentDTO _comment;

		private readonly int _userId;

		public AddCommentCommand(ICommentService commentService,
			IUserService userService,
			ITripService tripService,
			CreateCommentDTO comment,
			int userId)
		{
			_commentService = commentService;
			_userService = userService;
			_tripService = tripService;
			_userId = userId;
			_comment = comment;
		}

		public async Task ExecuteAsync()
		{
			Trip? trip = await _tripService.GetByIdAsync(_comment.TripId);
			User? user = await _userService.GetByIdAsync(_userId);
			await _commentService.AddCommentAsync(_comment, trip, user);
		}
	}
}
