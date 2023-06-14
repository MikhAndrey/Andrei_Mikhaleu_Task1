using TripsServiceBLL.DTO.Comments;
using TripsServiceBLL.Infrastructure;
using TripsServiceBLL.Interfaces;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Commands.Comments
{
    public class AddCommentCommand : IAsyncCommand
    {
        private readonly ICommentService _commentService;

        private readonly CreateCommentDTO _comment;

        private readonly int _userId;

        public AddCommentCommand(ICommentService commentService,
            CreateCommentDTO comment,
            int userId)
        {
            _commentService = commentService;
            _userId = userId;
            _comment = comment;
        }

        public async Task ExecuteAsync()
        {
            await _commentService.AddCommentAsync(_comment, _userId);
        }
    }
}
