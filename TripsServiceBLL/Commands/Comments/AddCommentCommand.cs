using TripsServiceBLL.DTO;
using TripsServiceBLL.Services;
using TripsServiceDAL.Entities;
using TripsServiceBLL.Infrastructure;

namespace TripsServiceBLL.Commands.Comments
{
    public class AddCommentCommand : IAsyncCommand
    {
        private readonly CommentService _commentService;

        private readonly UserService _userService;

        private readonly CommentDTO _comment;

        private readonly string _userName;

        public AddCommentCommand(CommentService commentService, UserService userService, CommentDTO comment, string userName)
        {
            _commentService = commentService;
            _userService = userService;
            _userName = userName;
            _comment = comment;
        }

        public async Task ExecuteAsync()
        {
            User? user = await _userService.GetByUserNameAsync(_userName);
            if (user == null)
                throw new ValidationException("User was not found", "");
            await _commentService.AddCommentAsync(_comment, user);
        }
    }
}
