using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripsServiceBLL.Commands.Trips;
using TripsServiceBLL.DTO.Comments;
using TripsServiceBLL.DTO.Trips;
using TripsServiceBLL.Helpers;
using TripsServiceBLL.Infrastructure.Exceptions;
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

        private readonly IMapper _mapper;

        private readonly CreateTripCommandAsync _createTripCommand;

        private readonly DeleteTripCommandAsync _deleteTripCommand;

        private readonly EditTripCommandAsync _editTripCommand;

        private readonly EditPastTripCommandAsync _editPastTripCommand;

        public TripsController(
            ICommentService service,
            IRoutePointService routePointService,
            IImageService imageService,
            ITripService tripService,
            IUserService userService,
            IWebHostEnvironment environment,
            IMapper mapper,
            CreateTripCommandAsync createTripCommand,
            DeleteTripCommandAsync deleteTripCommand,
            EditTripCommandAsync editTripCommand,
            EditPastTripCommandAsync editPastTripCommand
            )
        {
            _commentService = service;
            _routePointService = routePointService;
            _imageService = imageService;
            _tripService = tripService;
            _userService = userService;
            _environment = environment;
            _mapper = mapper;
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
            }
            return View(trip);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            int userId;
            try
            {
                userId = UserHelper.GetUserIdFromClaims(HttpContext.User.Claims);
                IQueryable<ReadTripDTO> trips = _tripService.GetTripsByUserId(userId);
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
            int userId;
            try
            {
                userId = UserHelper.GetUserIdFromClaims(HttpContext.User.Claims);
                IQueryable<ReadTripDTO> trips = _tripService.GetHistoryOfTripsByUserId(userId);
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
            int userId;
            try
            {
                userId = UserHelper.GetUserIdFromClaims(HttpContext.User.Claims);
                IQueryable<ReadTripDTOExtended> trips = _tripService.GetOthersPublicTrips(userId);
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
                int userId = UserHelper.GetUserIdFromClaims(HttpContext.User.Claims);
                TripDetailsDTO trip = await _tripService.GetTripDetailsAsync(id, userId);
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
            return RedirectToAction(nameof(Details), new { id });
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
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(CreateCommentDTO comment)
        {
            if (ModelState.IsValid)
            {
                int userId;
                try
                {
                    userId = UserHelper.GetUserIdFromClaims(HttpContext.User.Claims);
                    await _commentService.AddCommentAsync(comment, userId);
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
            return RedirectToAction(nameof(Details), new { id = comment.TripId });
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
            int userId;
            try
            {
                userId = UserHelper.GetUserIdFromClaims(HttpContext.User.Claims);
                await _imageService.DeleteByIdAsync(imageId, tripId, userId, _environment.WebRootPath);
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
}
