using AutoMapper;
using TripsServiceBLL.DTO.Feedbacks;
using TripsServiceBLL.Interfaces;

namespace TripsServiceBLL.Infrastructure.ValueResolvers
{
    public class FeedbackUserResolver : IValueResolver<CreateFeedbackDTO, ReadFeedbackDTO, string?>
    {
        private readonly IUserService _userService;

        public FeedbackUserResolver(IUserService userService)
        {
            _userService = userService;
        }

        public string? Resolve(CreateFeedbackDTO createFeedbackDTO, ReadFeedbackDTO readFeedbackDTO, string? feedbackUserName, ResolutionContext context)
        {
            return _userService.GetCurrentUserName();
        }
    }
}

