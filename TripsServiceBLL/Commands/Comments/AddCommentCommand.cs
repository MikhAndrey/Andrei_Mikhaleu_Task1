using TripsServiceBLL.DTO.Comments;
using TripsServiceBLL.Infrastructure;
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
            bool tripExists = _tripService.Exists(_comment.TripId);
            if (!tripExists)
                throw new EntityNotFoundException("Required trip was not found");
            bool userExists = _userService.Exists(_userId);
            if (!userExists)
                throw new EntityNotFoundException("Required user was not found");
            await _commentService.AddCommentAsync(_comment, _userId);
        }
    }
}
