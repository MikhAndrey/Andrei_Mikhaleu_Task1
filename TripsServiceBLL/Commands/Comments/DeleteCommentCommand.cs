using TripsServiceBLL.Interfaces;

namespace TripsServiceBLL.Commands.Comments
{
	public class DeleteCommentCommand : IAsyncCommand
	{
		private readonly ICommentService _commentService;

		private readonly int _commentId;

		public DeleteCommentCommand(ICommentService commentService, int commentId)
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
