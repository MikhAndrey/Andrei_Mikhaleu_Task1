using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripsServiceBLL.DTO.Comments;
using TripsServiceBLL.Interfaces;
using TripsServiceDAL.Infrastructure.Exceptions;

namespace Andrei_Mikhaleu_Task1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }
    
    [HttpPost("add")]
    public async Task<IActionResult> AddComment(CreateCommentDTO comment)
    {
        if (ModelState.IsValid)
        {
            try
            {
                CommentDTO result = await _commentService.AddCommentAsync(comment);
                return Ok(result);
            }
            catch (ArgumentNullException)
            {
                return Unauthorized();
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        return BadRequest(ModelState);
    }

    [Authorize]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        try
        {
            await _commentService.DeleteCommentAsync(id);
            return Ok();
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}

