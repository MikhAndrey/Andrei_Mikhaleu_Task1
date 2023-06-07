using TripsServiceBLL.Services;

namespace TripsServiceBLL.Commands.Comments
{
    public class DeleteCommentCommand : IAsyncCommand
    {
        private readonly CommentService _commentService;

        private readonly int _commentId;

        public DeleteCommentCommand(CommentService commentService, int commentId)
        {
            _commentService = commentService;
            _commentId = commentId;
        }

        public async Task ExecuteAsync()
        {
            await _commentService.DeleteCommentAsync(_commentId);
        }
    }
}
