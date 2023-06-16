using Microsoft.AspNetCore.Mvc;
using TripsServiceBLL.Commands.Feedbacks;
using TripsServiceBLL.DTO.Feedbacks;
using TripsServiceBLL.Infrastructure;
using TripsServiceBLL.Interfaces;

namespace Andrei_Mikhaleu_Task1.Controllers
{
	public class FeedbacksController : Controller
	{
		private IFeedbackService _feedbackService;

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
				return RedirectToAction("Index", "Trips");
			} catch (EntityNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
		}
	}
}
