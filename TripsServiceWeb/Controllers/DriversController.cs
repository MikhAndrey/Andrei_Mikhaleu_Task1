using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripsServiceBLL.DTO.Drivers;
using TripsServiceBLL.Interfaces;
using TripsServiceDAL.Infrastructure.Exceptions;

namespace Andrei_Mikhaleu_Task1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DriversController : ControllerBase
{
    private readonly IDriverService _driverService;

    public DriversController(
        IDriverService driverService
    )
    {
        _driverService = driverService;
    }
    
    [Authorize]
    [HttpGet("list")]
    public IActionResult List()
    {
        IEnumerable<ReadDriverDTO> drivers = _driverService.GetDriversOverall();
        return Ok(drivers);
    }

    [Authorize]
    [HttpGet("details/{id}")]
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            DriverDetailsDTO driver = await _driverService.GetDriverDetailsAsync(id);
            return Ok(driver);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
