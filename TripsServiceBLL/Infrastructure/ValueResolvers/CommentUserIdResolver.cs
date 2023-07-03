using AutoMapper;
using TripsServiceBLL.DTO.Comments;
using TripsServiceBLL.Interfaces;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Infrastructure.ValueResolvers
{
    public class CommentUserIdResolver : IValueResolver<CreateCommentDTO, Comment, int>
    {
        private readonly IUserService _userService;

        public CommentUserIdResolver(IUserService userService)
        {
            _userService = userService;
        }

        public int Resolve(CreateCommentDTO dto, Comment comment, int commentUserId, ResolutionContext context)
        {
            return _userService.GetCurrentUserId();
        }
    }
}
