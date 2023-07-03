using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripsServiceBLL.Commands.Trips;
using TripsServiceBLL.DTO.Comments;
using TripsServiceBLL.DTO.Trips;
using TripsServiceBLL.Infrastructure.Exceptions;
using TripsServiceBLL.Interfaces;
using TripsServiceDAL.Infrastructure.Exceptions;

namespace Andrei_Mikhaleu_Task1.Controllers;

public class TripsController : Controller
{
	private readonly ICommentService _commentService;
	private readonly IImageService _imageService;
	private readonly ITripService _tripService;

	private readonly IWebHostEnvironment _environment;
	
	private readonly CreateTripCommandAsync _createTripCommand;
	private readonly DeleteTripCommandAsync _deleteTripCommand;
	private readonly EditPastTripCommandAsync _editPastTripCommand;
	private readonly EditTripCommandAsync _editTripCommand;
	
	public TripsController(
		ICommentService service,
		IImageService imageService,
		ITripService tripService,
		IWebHostEnvironment environment,
		CreateTripCommandAsync createTripCommand,
		DeleteTripCommandAsync deleteTripCommand,
		EditTripCommandAsync editTripCommand,
		EditPastTripCommandAsync editPastTripCommand
	)
	{
		_commentService = service;
		_imageService = imageService;
		_tripService = tripService;
		_environment = environment;
		_createTripCommand = createTripCommand;
		_deleteTripCommand = deleteTripCommand;
		_editTripCommand = editTripCommand;
		_editPastTripCommand = editPastTripCommand;
	}

	[Authorize]
	[HttpGet]
	public IActionResult Create()
	{
		return View();
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(CreateTripDTO trip)
	{
		if (ModelState.IsValid)
		{
			try
			{
				await _createTripCommand.ExecuteAsync(trip);
				return RedirectToAction(nameof(Index));
			}
			catch (ArgumentNullException)
			{
				return RedirectToAction("Login", "Account");
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

		return View(trip);
	}

	[Authorize]
	[HttpGet]
	public IActionResult Index()
	{
		try
		{
			IQueryable<ReadTripDTO> trips = _tripService.GetCurrentUserTrips();
			return View(trips);
		}
		catch (ArgumentNullException)
		{
			return RedirectToAction("Login", "Account");
		}
		catch (EntityNotFoundException ex)
		{
			return NotFound(ex.Message);
		}
	}

	[Authorize]
	[HttpGet]
	public IActionResult History()
	{
		try
		{
			IQueryable<ReadTripDTO> trips = _tripService.GetCurrentUserHistoryOfTrips();
			return View(trips);
		}
		catch (ArgumentNullException)
		{
			return RedirectToAction("Login", "Account");
		}
		catch (EntityNotFoundException ex)
		{
			return NotFound(ex.Message);
		}
	}

	[Authorize]
	[HttpGet]
	public IActionResult Public()
	{
		try
		{
			IQueryable<ReadTripDTOExtended> trips = _tripService.GetOthersPublicTrips();
			return View(trips);
		}
		catch (ArgumentNullException)
		{
			return RedirectToAction("Login", "Account");
		}
		catch (EntityNotFoundException ex)
		{
			return NotFound(ex.Message);
		}
	}

	[Authorize]
	[HttpDelete]
	public async Task<IActionResult> Delete(int id)
	{
		try
		{
			await _deleteTripCommand.ExecuteAsync(id);
		}
		catch (EntityNotFoundException ex)
		{
			return NotFound(ex.Message);
		}
		catch (DbOperationException ex)
		{
			return BadRequest(ex.Message);
		}

		return Ok();
	}

	[Authorize]
	[HttpGet]
	public async Task<IActionResult> Edit(int id)
	{
		try
		{
			EditTripDTO trip = await _tripService.GetTripForEditingAsync(id);
			return View(trip);
		}
		catch (EntityNotFoundException ex)
		{
			return NotFound(ex.Message);
		}
	}

	[Authorize]
	[HttpGet]
	public async Task<IActionResult> EditPast(int id)
	{
		try
		{
			EditPastTripDTO trip = await _tripService.GetPastTripForEditingAsync(id);
			return View(trip);
		}
		catch (EntityNotFoundException ex)
		{
			return NotFound(ex.Message);
		}
	}

	[HttpPut]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(int id, EditTripDTO trip)
	{
		if (ModelState.IsValid)
		{
			try
			{
				await _editTripCommand.ExecuteAsync(trip);
			}
			catch (EntityNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
			catch (DbOperationException ex)
			{
				return BadRequest(ex.Message);
			}

			return RedirectToAction(nameof(Index));
		}

		return View(trip);
	}

	[HttpPut]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> EditPast(int id, EditPastTripDTO trip)
	{
		if (ModelState.IsValid)
		{
			try
			{
				await _editPastTripCommand.ExecuteAsync(trip);
			}
			catch (EntityNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
			catch (DbOperationException ex)
			{
				return BadRequest(ex.Message);
			}

			return RedirectToAction(nameof(Index));
		}

		return View(trip);
	}

	[Authorize]
	[HttpGet]
	public async Task<IActionResult> Details(int id)
	{
		try
		{
			TripDetailsDTO trip = await _tripService.GetTripDetailsAsync(id);
			return View(trip);
		}
		catch (ArgumentNullException)
		{
			return RedirectToAction("Login", "Account");
		}
		catch (EntityNotFoundException ex)
		{
			return NotFound(ex.Message);
		}
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> StartTrip(int id)
	{
		try
		{
			await _tripService.StartTripAsync(id);
		}
		catch (EntityNotFoundException ex)
		{
			return NotFound(ex.Message);
		}

		return RedirectToAction(nameof(Details), new {id});
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> EndTrip(int id)
	{
		try
		{
			await _tripService.EndTripAsync(id);
		}
		catch (EntityNotFoundException ex)
		{
			return NotFound(ex.Message);
		}

		return RedirectToAction(nameof(Details), new {id});
	}

	[HttpPost]
	public async Task<IActionResult> AddComment(CreateCommentDTO comment)
	{
		if (ModelState.IsValid)
		{
			try
			{
				await _commentService.AddCommentAsync(comment);
			}
			catch (ArgumentNullException)
			{
				return RedirectToAction("Login", "Account");
			}
			catch (EntityNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
		}

		return RedirectToAction(nameof(Details), new {id = comment.TripId});
	}

	[Authorize]
	[HttpDelete]
	public async Task<IActionResult> DeleteComment(int commentId)
	{
		try
		{
			await _commentService.DeleteCommentAsync(commentId);
		}
		catch (EntityNotFoundException ex)
		{
			return NotFound(ex.Message);
		}

		return Ok();
	}

	[Authorize]
	[HttpDelete]
	public async Task<IActionResult> DeleteImage(int imageId, int tripId)
	{
		try
		{
			await _imageService.DeleteByIdAsync(imageId, tripId, _environment.WebRootPath);
		}
		catch (ArgumentNullException)
		{
			return RedirectToAction("Login", "Account");
		}
		catch (EntityNotFoundException ex)
		{
			return NotFound(ex.Message);
		}

		return Ok();
	}
}