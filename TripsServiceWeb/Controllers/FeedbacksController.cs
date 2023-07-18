using Andrei_Mikhaleu_Task1.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TripsServiceBLL.DTO.Feedbacks;
using TripsServiceBLL.Interfaces;
using TripsServiceDAL.Infrastructure.Exceptions;

namespace Andrei_Mikhaleu_Task1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FeedbacksController : ControllerBase
{
    private readonly IHubContext<FeedbacksHub> _feedbackHubContext;
    private readonly IFeedbackService _feedbackService;

    public FeedbacksController(
        IFeedbackService feedbackService,
        IHubContext<FeedbacksHub> feedbackHubContext)
    {
        _feedbackService = feedbackService;
        _feedbackHubContext = feedbackHubContext;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateFeedbackDTO feedback)
    {
        try
        {
            ReadFeedbackDTO addedFeedback = await _feedbackService.AddAsync(feedback);
            await _feedbackHubContext.Clients.All.SendAsync("FeedbackCreate", addedFeedback);
            return Ok();
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update(UpdateFeedbackDTO dto)
    {
        try
        {
            await _feedbackService.UpdateAsync(dto);
            await _feedbackHubContext.Clients.All.SendAsync("FeedbackUpdate", dto);
            return Ok();
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("delete/{id}")]
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

