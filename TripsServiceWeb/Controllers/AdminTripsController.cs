using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripsServiceBLL.Commands.Trips;
using TripsServiceBLL.DTO.Trips;
using TripsServiceBLL.Infrastructure.Exceptions;
using TripsServiceBLL.Interfaces;
using TripsServiceDAL.Infrastructure.Exceptions;

namespace Andrei_Mikhaleu_Task1.Controllers;

[Route("api/admin/trips")]
[Authorize(Roles = "Admin")]
[ApiController]
public class AdminTripsController : ControllerBase
{
	private readonly ITripService _tripService;

	private readonly CreateTripCommandAsync _createTripCommand;
	private readonly EditTripCommandAsync _editTripCommand;

	public AdminTripsController(
		ITripService tripService,
		CreateTripCommandAsync createTripCommand,
		EditTripCommandAsync editTripCommand
	)
	{
		_tripService = tripService;
		_createTripCommand = createTripCommand;
		_editTripCommand = editTripCommand;
	}

	[HttpPost("create")]
	public async Task<IActionResult> Create([FromForm] AdminCreateTripDTO trip)
	{
		if (ModelState.IsValid)
		{
			try
			{
				await _createTripCommand.ExecuteAsync(trip);
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
			catch (DbOperationException ex)
			{
				return BadRequest(ex.Message);
			}
		}

		return BadRequest(ModelState);
	}
	
	[HttpGet("index")]
	public IActionResult Index()
	{
		try
		{
			IEnumerable<ReadTripDTOExtended> trips = _tripService.GetAllTrips();
			return Ok(trips);
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
	
	[HttpGet("edit/{id}")]
	public async Task<IActionResult> Edit(int id)
	{
		try
		{
			AdminEditTripDTO trip = await _tripService.GetTripForEditingByAdminAsync(id);
			return Ok(trip);
		}
		catch (EntityNotFoundException ex)
		{
			return NotFound(ex.Message);
		}
	}
	
	[HttpPut("edit/{id}")]
	public async Task<IActionResult> Edit(int id, [FromForm] AdminEditTripDTO trip)
	{
		if (ModelState.IsValid)
		{
			try
			{
				await _editTripCommand.ExecuteAsync(trip);
				return Ok();
			}
			catch (EntityNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
			catch (DbOperationException ex)
			{
				return BadRequest(ex.Message);
			}
		}

		return BadRequest(ModelState);
	}
}

