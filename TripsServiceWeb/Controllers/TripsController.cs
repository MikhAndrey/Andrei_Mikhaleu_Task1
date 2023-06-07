using Andrei_Mikhaleu_Task1.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripsServiceBLL.DTO;
using TripsServiceBLL.Services;
using TripsServiceBLL.Commands.Trips;
using TripsServiceBLL.Commands;
using TripsServiceBLL.Infrastructure;
using TripsServiceBLL.Commands.Comments;
using TripsServiceBLL.Commands.Images;
using TripsServiceDAL.Entities;

namespace Andrei_Mikhaleu_Task1.Controllers
{
    public class TripsController : Controller
    {

        private readonly CommentService _commentService;

        private readonly RoutePointService _routePointService;

        private readonly ImageService _imageService;

        private readonly TripService _tripService;

        private readonly UserService _userService;

        private readonly IWebHostEnvironment _environment;

        public TripsController(
            CommentService service,
            RoutePointService routePointService,
            ImageService imageService,
            TripService tripService,
            UserService userService,
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
                string? userName = HttpContext?.User?.Identity?.Name;
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
                AsyncCommandInvoker invoker = new()
                {
                    Command = new CreateTripCommand(trip, images, _routePointService, _imageService,
                    _tripService, _userService, _environment.WebRootPath, routePoints, userName)
                };
                try
                {
                    await invoker.ExecuteCommandAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (ValidationException ex)
                {
                    return NotFound(ex.Message);
                }
            }

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            string? userName = HttpContext?.User?.Identity?.Name;
            AsyncGenericCommandInvoker<List<TripDTO>> invoker = new()
            {
                Command = new GetUserTripsCommand(_tripService, _userService, userName)
            };
            try
            {
                List<TripDTO> trips = await invoker.ExecuteCommandAsync();
                List<TripViewModel> tripModels = trips.Select(t => new TripViewModel(t)).ToList();
                return View(tripModels);
            }
            catch (ValidationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> History()
        {
            string? userName = HttpContext?.User?.Identity?.Name;
            AsyncGenericCommandInvoker<List<TripDTO>> invoker = new()
            {
                Command = new GetTripsHistoryCommand(_tripService, _userService, userName)
            };
            try
            {
                List<TripDTO> trips = await invoker.ExecuteCommandAsync();
                List<TripViewModel> tripModels = trips.Select(t => new TripViewModel(t)).ToList();
                return View(tripModels);
            }
            catch (ValidationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Public()
        {
            string? userName = HttpContext?.User?.Identity?.Name;
            AsyncGenericCommandInvoker<List<TripDTO>> invoker = new()
            {
                Command = new GetOthersPublicTripsCommand(_tripService, _userService, userName)
            };
            try
            {
                List<TripDTO> trips = await invoker.ExecuteCommandAsync();
                List<TripPublicViewModel> tripModels = trips.Select(t => new TripPublicViewModel(t)).ToList();
                return View(tripModels);
            }
            catch (ValidationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            AsyncCommandInvoker invoker = new()
            {
                Command = new DeleteTripCommand(id, _tripService, _imageService)
            };
            try
            {
                await invoker.ExecuteCommandAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (ValidationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            AsyncGenericCommandInvoker<ExtendedExistingTripDTO> invoker = new()
            {
                Command = new GetTripByIdCommand(_tripService, id)
            };
            try
            {
                ExtendedExistingTripDTO trip = await invoker.ExecuteCommandAsync();
                EditTripViewModel viewModel = new(trip);
                return View(viewModel);
            }
            catch (ValidationException ex)
            {
                return NotFound(ex.Message);
            }
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

                AsyncCommandInvoker invoker = new()
                {
                    Command = new EditTripCommand(trip, id, images, _routePointService, _imageService,
                    _tripService, _environment.WebRootPath, routePoints)
                };
                try
                {
                    await invoker.ExecuteCommandAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (ValidationException ex)
                {
                    return NotFound(ex.Message);
                }
            }

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            string? userName = HttpContext?.User?.Identity?.Name;
            AsyncGenericCommandInvoker<TripDTO> invoker = new()
            {
                Command = new GetTripDetailsCommand(_tripService, _userService, id, userName)
            };
            try
            {
                TripDTO trip = await invoker.ExecuteCommandAsync();
                TripDetailsViewModel viewModel = new(trip);
                return View(viewModel);
            }
            catch (ValidationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> StartTrip(int id)
        {
            AsyncCommandInvoker invoker = new()
            {
                Command = new StartTripCommand(_tripService, id)
            };
            try
            {
                await invoker.ExecuteCommandAsync();
                return RedirectToAction(nameof(Details), new { id });
            }
            catch (ValidationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EndTrip(int id)
        {
            AsyncCommandInvoker invoker = new()
            {
                Command = new EndTripCommand(_tripService, id)
            };
            try
            {
                await invoker.ExecuteCommandAsync();
                return RedirectToAction(nameof(Details), new { id });
            }
            catch (ValidationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddComment(NewCommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                string? userName = HttpContext?.User?.Identity?.Name;
                if (userName != null)
                {
                    CommentDTO comment = new()
                    {
                        Message = model.Message,
                        TripId = model.TripId
                    };
                    AsyncCommandInvoker invoker = new()
                    {
                        Command = new AddCommentCommand(_commentService, _userService, comment, userName)
                    };
                    try
                    {
                        await invoker.ExecuteCommandAsync();
                    }
                    catch (ValidationException ex)
                    {
                        return NotFound(ex.Message);
                    }
                }
            }
            return RedirectToAction(nameof(Details), new { id = model.TripId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteComment(int tripId, int commentId)
        {
            AsyncCommandInvoker invoker = new()
            {
                Command = new DeleteCommentCommand(_commentService, commentId)
            };
            await invoker.ExecuteCommandAsync();
            return RedirectToAction(nameof(Details), new { id = tripId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            AsyncCommandInvoker invoker = new()
            {
                Command = new DeleteImageCommand(_environment.WebRootPath, imageId, _imageService)
            };
            try
            {
                await invoker.ExecuteCommandAsync();
                return NoContent();
            }
            catch (ValidationException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
