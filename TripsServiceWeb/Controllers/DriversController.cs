using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripsServiceBLL.DTO.Drivers;
using TripsServiceBLL.Interfaces;
using TripsServiceDAL.Infrastructure.Exceptions;

namespace Andrei_Mikhaleu_Task1.Controllers;

public class DriversController : Controller
{
	private readonly IDriverService _driverService;

	public DriversController(
		IDriverService driverService
	)
	{
		_driverService = driverService;
	}

	[Authorize]
	[HttpGet]
	public IActionResult Index()
	{
		IEnumerable<ReadDriverDTO> drivers = _driverService.GetDriversOverall();
		return View(drivers);
	}

	[Authorize]
	[HttpGet]
	public IActionResult List()
	{
		IEnumerable<ReadDriverDTO> drivers = _driverService.GetDriversOverall();
		JsonSerializerOptions options = new()
		{
			ReferenceHandler = ReferenceHandler.Preserve
		};
		return Json(drivers, options);
	}

	[Authorize]
	[HttpGet]
	public async Task<IActionResult> Details(int id)
	{
		try
		{
			DriverDetailsDTO driver = await _driverService.GetDriverDetailsAsync(id);
			return View(driver);
		}
		catch (EntityNotFoundException ex)
		{
			return NotFound(ex.Message);
		}
	}
}
