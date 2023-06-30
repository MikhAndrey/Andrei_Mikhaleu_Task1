using Microsoft.AspNetCore.Mvc;
using TripsServiceBLL.DTO.Feedbacks;
using TripsServiceBLL.Interfaces;
using TripsServiceDAL.Infrastructure.Exceptions;

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
				await _feedbackService.AddAsync(feedback);
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
				return Ok();
			}
			catch (EntityNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
		}
	}
}
