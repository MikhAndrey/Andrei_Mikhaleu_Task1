using Microsoft.AspNetCore.Mvc;
using TripsServiceBLL.Commands.Feedbacks;
using TripsServiceBLL.DTO.Feedbacks;
using TripsServiceBLL.Infrastructure;
using TripsServiceBLL.Interfaces;

namespace Andrei_Mikhaleu_Task1.Controllers
{
    public class FeedbacksController : Controller
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbacksController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateFeedbackDTO feedback)
        {
            try
            {
                await new CreateFeedbackCommand(_feedbackService, feedback).ExecuteAsync();
                return Redirect(Request.Headers["Referer"].ToString());
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
