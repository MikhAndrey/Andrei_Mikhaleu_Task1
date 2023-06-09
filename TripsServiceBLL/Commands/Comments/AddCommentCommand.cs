using TripsServiceDAL.Entities;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.DTO.Comments;

namespace TripsServiceBLL.Commands.Comments
{
    public class AddCommentCommand : IAsyncCommand
    {
        private readonly ICommentService _commentService;

        private readonly IUserService _userService;

        private readonly CommentDTO _comment;

        private readonly int _userId;

        public AddCommentCommand(ICommentService commentService, 
            IUserService userService, 
            CommentDTO comment, 
            int userId)
        {
            _commentService = commentService;
            _userService = userService;
            _userId = userId;
            _comment = comment;
        }

        public async Task ExecuteAsync()
        {
            User? user = await _userService.GetByIdAsync(_userId);
            if (user != null)
                await _commentService.AddCommentAsync(_comment, user);
        }
    }
}
