using TripsServiceBLL.DTO.Feedbacks;
using TripsServiceBLL.Interfaces;

namespace TripsServiceBLL.Commands.Feedbacks
{
    public class CreateFeedbackCommand : IAsyncCommand
    {
        private readonly IFeedbackService _feedbackService;

        private readonly CreateFeedbackDTO _feedback;

        public CreateFeedbackCommand(IFeedbackService feedbackService, CreateFeedbackDTO feedback)
        {
            _feedbackService = feedbackService;
            _feedback = feedback;
        }

        public async Task ExecuteAsync()
        {
            await _feedbackService.AddAsync(_feedback);
        }
    }
}
