using Andrei_Mikhaleu_Task1.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripsServiceBLL.Commands.Trips;
using TripsServiceBLL.Infrastructure;
using TripsServiceBLL.Commands.Comments;
using TripsServiceBLL.Commands.Images;
using TripsServiceBLL.DTO.Comments;
using TripsServiceBLL.DTO.Trips;
using TripsServiceBLL.Interfaces;

namespace Andrei_Mikhaleu_Task1.Controllers
{
    public class TripsController : Controller
    {

        private readonly ICommentService _commentService;

        private readonly IRoutePointService _routePointService;

        private readonly IImageService _imageService;

        private readonly ITripService _tripService;

        private readonly IUserService _userService;

        private readonly IWebHostEnvironment _environment;

        public TripsController(
            ICommentService service,
            IRoutePointService routePointService,
            IImageService imageService,
            ITripService tripService,
            IUserService userService,
            IWebHostEnvironment environment
            )
        {
            _commentService = service;
            _routePointService = routePointService;
            _imageService = imageService;
            _tripService = tripService;
            _userService = userService;
            _environment = environment;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewTripViewModel model, List<IFormFile> images, string routePoints)
        {
            if (ModelState.IsValid)
            {
				int userId = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value);
				NewTripDTO trip = new()
                {
                    Name = model.Name,
                    Description = model.Description,
                    Distance = model.Distance,
                    Public = model.Public,
                    StartTimeZoneOffset = model.StartTimeZoneOffset,
                    FinishTimeZoneOffset = model.FinishTimeZoneOffset,
                    StartTime = model.StartTime.AddSeconds(-model.StartTimeZoneOffset),
                    EndTime = model.EndTime.AddSeconds(-model.FinishTimeZoneOffset),
                };
                await new CreateTripCommand(trip, images, _routePointService, _imageService,
                    _tripService, _userService, _environment.WebRootPath, routePoints, userId).ExecuteAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
			int userId = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value);
            List<TripDTO> trips = await new GetUserTripsCommand(_tripService, userId).ExecuteAsync();
			List<TripViewModel> tripModels = trips.Select(t => new TripViewModel(t)).ToList();
            return View(tripModels);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> History()
        {
			int userId = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value);
			List<TripDTO> trips = await new GetTripsHistoryCommand(_tripService, userId).ExecuteAsync();
			List<TripViewModel> tripModels = trips.Select(t => new TripViewModel(t)).ToList();
			return View(tripModels);
		}

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Public()
        {
			int userId = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value);
			List<TripDTO> trips = await new GetOthersPublicTripsCommand(_tripService, userId).ExecuteAsync();
            List<TripPublicViewModel> tripModels = trips.Select(t => new TripPublicViewModel(t)).ToList();
            return View(tripModels);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await new DeleteTripCommand(id, _tripService, _imageService).ExecuteAsync();
			return RedirectToAction(nameof(Index));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ExtendedExistingTripDTO trip = await new GetTripByIdCommand(_tripService, id).ExecuteAsync();
            EditTripViewModel viewModel = new(trip);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditTripViewModel model, List<IFormFile> images, string routePoints)
        {
            if (ModelState.IsValid)
            {
                ExistingTripDTO trip = new()
                {
                    Description = model.Description,
                    Name = model.Name,
                    StartTime = model.StartTime.AddSeconds(-model.StartTimeZoneOffset),
                    EndTime = model.EndTime.AddSeconds(-model.FinishTimeZoneOffset),
                    Distance = model.Distance,
                    Public = model.Public,
                    StartTimeZoneOffset = model.StartTimeZoneOffset,
                    FinishTimeZoneOffset = model.FinishTimeZoneOffset
                };
                await new EditTripCommand(trip, id, images, _routePointService, _imageService,
                _tripService, _environment.WebRootPath, routePoints).ExecuteAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
			int userId = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value);
			TripDTO trip = await new GetTripDetailsCommand(_tripService, id, userId).ExecuteAsync();
            TripDetailsViewModel viewModel = new(trip);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartTrip(int id)
        {
            await new StartTripCommand(_tripService, id).ExecuteAsync();
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EndTrip(int id)
        {
			await new EndTripCommand(_tripService, id).ExecuteAsync();
			return RedirectToAction(nameof(Details), new { id });
		}

        [HttpPost]
        public async Task<IActionResult> AddComment(NewCommentViewModel model)
        {
            if (ModelState.IsValid)
            {
				int userId = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value);

				CommentDTO comment = new()
                {
                    Message = model.Message,
                    TripId = model.TripId
                };
                await new AddCommentCommand(_commentService, _userService, comment, userId).ExecuteAsync();
            }
            return RedirectToAction(nameof(Details), new { id = model.TripId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteComment(int tripId, int commentId)
        {
            await new DeleteCommentCommand(_commentService, commentId).ExecuteAsync();
            return RedirectToAction(nameof(Details), new { id = tripId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            try
            {
                await new DeleteImageCommand(_environment.WebRootPath, imageId, _imageService).ExecuteAsync();
				return NoContent();
            }
            catch (ValidationException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
