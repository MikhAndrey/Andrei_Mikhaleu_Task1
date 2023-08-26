using Microsoft.AspNetCore.Mvc;
using TripsServiceBLL.Interfaces;

namespace Andrei_Mikhaleu_Task1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificationsController : ControllerBase
{
    private INotificationsService _notificationsService;
    private IUserService _userService;

    public NotificationsController(INotificationsService notificationsService, IUserService userService)
    {
        _notificationsService = notificationsService;
        _userService = userService;
    }
    
    [HttpDelete("delete/{id}")]
    public IActionResult Delete(int id)
    {
        int userId = _userService.GetCurrentUserId();
        _notificationsService.Delete(userId.ToString(), id);
        return Ok();
    }
}
