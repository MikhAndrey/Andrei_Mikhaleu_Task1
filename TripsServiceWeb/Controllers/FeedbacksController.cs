using Andrei_Mikhaleu_Task1.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TripsServiceBLL.DTO.Feedbacks;
using TripsServiceBLL.Interfaces;
using TripsServiceDAL.Infrastructure.Exceptions;

namespace Andrei_Mikhaleu_Task1.Controllers
{
    public class FeedbacksController : Controller
    {
        private readonly IFeedbackService _feedbackService;

        private readonly IHubContext<FeedbacksHub> _feedbackHubContext;

        public FeedbacksController(
			IFeedbackService feedbackService, 
			IHubContext<FeedbacksHub> feedbackHubContext)
        {
            _feedbackService = feedbackService;
			_feedbackHubContext = feedbackHubContext;
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateFeedbackDTO feedback)
		{
			try
			{
				await _feedbackService.AddAsync(feedback);
                await _feedbackHubContext.Clients.All.SendAsync("FeedbackCreate", feedback);
                return Redirect(Request.Headers["Referer"].ToString());
			}
			catch (EntityNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
		}

		[HttpDelete]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				await _feedbackService.DeleteAsync(id);
                await _feedbackHubContext.Clients.All.SendAsync("FeedbackDelete", id);
                return Ok();
			}
			catch (EntityNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
		}
	}
}
