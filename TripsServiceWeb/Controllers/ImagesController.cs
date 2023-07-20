using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripsServiceBLL.Interfaces;
using TripsServiceDAL.Infrastructure.Exceptions;

namespace Andrei_Mikhaleu_Task1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImagesController : ControllerBase
{
    private readonly IImageService _imageService;

    public ImagesController(IImageService imageService)
    {
        _imageService = imageService;
    }
    
    [Authorize]
    [HttpDelete("delete/{imageId}/{tripId}")]
    public async Task<IActionResult> DeleteImage(int imageId, int tripId)
    {
        try
        {
            await _imageService.DeleteByIdAsync(imageId, tripId);
            return Ok();
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
}
