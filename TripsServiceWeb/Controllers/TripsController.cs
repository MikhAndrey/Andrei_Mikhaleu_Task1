using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripsServiceBLL.Commands.Trips;
using TripsServiceBLL.DTO.Trips;
using TripsServiceBLL.Infrastructure.Exceptions;
using TripsServiceBLL.Interfaces;
using TripsServiceDAL.Infrastructure.Exceptions;

namespace Andrei_Mikhaleu_Task1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TripsController : ControllerBase
{
	private readonly ITripService _tripService;

	private readonly CreateTripCommandAsync _createTripCommand;
	private readonly DeleteTripCommandAsync _deleteTripCommand;
	private readonly EditPastTripCommandAsync _editPastTripCommand;
	private readonly EditTripCommandAsync _editTripCommand;

	public TripsController(
		ITripService tripService,
		CreateTripCommandAsync createTripCommand,
		DeleteTripCommandAsync deleteTripCommand,
		EditTripCommandAsync editTripCommand,
		EditPastTripCommandAsync editPastTripCommand
	)
	{
		_tripService = tripService;
		_createTripCommand = createTripCommand;
		_deleteTripCommand = deleteTripCommand;
		_editTripCommand = editTripCommand;
		_editPastTripCommand = editPastTripCommand;
	}
	
	[HttpPost("create")]
	public async Task<IActionResult> Create([FromForm] CreateTripDTO trip)
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

	[Authorize]
	[HttpGet("index")]
	public IActionResult Index()
	{
		try
		{
			IEnumerable<ReadTripDTO> trips = _tripService.GetCurrentUserTrips();
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

	[Authorize]
	[HttpGet("history")]
	public IActionResult History()
	{
		try
		{
			IEnumerable<ReadTripDTO> trips = _tripService.GetCurrentUserHistoryOfTrips();
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

	[Authorize]
	[HttpGet("public")]
	public IActionResult Public()
	{
		try
		{
			IEnumerable<ReadTripDTOExtended> trips = _tripService.GetOthersPublicTrips();
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

	[Authorize]
	[HttpDelete("delete/{id}")]
	public async Task<IActionResult> Delete(int id)
	{
		try
		{
			await _deleteTripCommand.ExecuteAsync(id);
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
	
	[HttpPut("edit/current/{id}")]
	public async Task<IActionResult> Edit(int id, EditTripDTO trip)
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

	[HttpPut("edit/past/{id}")]
	public async Task<IActionResult> EditPast(int id, EditPastTripDTO trip)
	{
		if (ModelState.IsValid)
		{
			try
			{
				await _editPastTripCommand.ExecuteAsync(trip);
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
	
	[Authorize]
	[HttpGet("details/{id}")]
	public async Task<IActionResult> Details(int id)
	{
		try
		{
			TripDetailsDTO trip = await _tripService.GetTripDetailsAsync(id);
			return Ok(trip);
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
	
	[HttpPost("start/{id}")]
	public async Task<IActionResult> StartTrip(int id)
	{
		try
		{
			await _tripService.StartTripAsync(id);
			return Ok();
		}
		catch (EntityNotFoundException ex)
		{
			return NotFound(ex.Message);
		}
	}

	[HttpPost("finish/{id}")]
	public async Task<IActionResult> EndTrip(int id)
	{
		try
		{
			await _tripService.EndTripAsync(id);
			return Ok();
		}
		catch (EntityNotFoundException ex)
		{
			return NotFound(ex.Message);
		}
	}
}
