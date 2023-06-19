using Andrei_Mikhaleu_Task1.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripsServiceBLL.Commands.Comments;
using TripsServiceBLL.Commands.Images;
using TripsServiceBLL.Commands.Trips;
using TripsServiceBLL.DTO.Comments;
using TripsServiceBLL.DTO.Trips;
using TripsServiceBLL.Infrastructure;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Utils;

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

        public TripsController(
            ICommentService service,
            IRoutePointService routePointService,
            IImageService imageService,
            ITripService tripService,
            IUserService userService,
            IWebHostEnvironment environment,
            IMapper mapper
            )
        {
            _commentService = service;
            _routePointService = routePointService;
            _imageService = imageService;
            _tripService = tripService;
            _userService = userService;
            _environment = environment;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTripDTO trip, List<IFormFile> images, string routePoints)
        {
            if (ModelState.IsValid)
            {
                int userId;
                try
                {
                    userId = UserHelper.GetUserIdFromClaims(HttpContext.User.Claims);
                }
                catch (ArgumentNullException)
                {
                    return RedirectToAction("Login", "Account");
                }
                await new CreateTripCommand(trip, images, _routePointService, _imageService,
                    _tripService, _userService, _mapper, _environment.WebRootPath, routePoints, userId).ExecuteAsync();
                return RedirectToAction(nameof(Index));
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
                IQueryable<ReadTripDTO> trips = new GetUserTripsCommand(_tripService, userId).Execute();
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
                IQueryable<ReadTripDTO> trips = new GetTripsHistoryCommand(_tripService, userId).Execute();
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
                IQueryable<ReadTripDTOExtended> trips = new GetOthersPublicTripsCommand(_tripService, userId).Execute();
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
                await new DeleteTripCommand(id, _tripService, _imageService, _environment.WebRootPath).ExecuteAsync();
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
                EditTripDTO trip = await new GetTripByIdCommand(_tripService, id).ExecuteAsync();
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
                EditPastTripDTO trip = await new GetPastTripByIdCommand(_tripService, id).ExecuteAsync();
                return View(trip);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditTripDTO trip, List<IFormFile> images, string routePoints)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await new EditTripCommand(trip, id, images, _routePointService, _imageService,
                    _tripService, _environment.WebRootPath, routePoints, _mapper).ExecuteAsync();
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
        public async Task<IActionResult> EditPast(int id, EditPastTripDTO trip, List<IFormFile> images)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await new EditPastTripCommand(trip, id, images, _imageService,
                    _tripService, _environment.WebRootPath, _mapper).ExecuteAsync();
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
                int userId = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == Constants.UserIdClaimName)?.Value);
                TripDetailsDTO trip = await new GetTripDetailsCommand(_tripService, id, userId).ExecuteAsync();
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
                await new StartTripCommand(_tripService, id).ExecuteAsync();
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
                await new EndTripCommand(_tripService, id).ExecuteAsync();
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
                    await new AddCommentCommand(_commentService, comment, userId).ExecuteAsync();
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
                await new DeleteCommentCommand(_commentService, commentId).ExecuteAsync();
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
                await new DeleteImageCommand(_environment.WebRootPath, imageId, tripId, userId, _imageService).ExecuteAsync();
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
