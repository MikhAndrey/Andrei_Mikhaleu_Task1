using Andrei_Mikhaleu_Task1.Common;
using Andrei_Mikhaleu_Task1.Models.ViewModels;
using Andrei_Mikhaleu_Task1.Models.Entities.Common;
using Andrei_Mikhaleu_Task1.Models.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Andrei_Mikhaleu_Task1.Controllers
{
    public class TripsController : Controller
    {

        private readonly TripRepository _tripRepository;

		private readonly UserSettings _userSettings;

		private readonly CommentRepository _commentRepository;

        private readonly ImageRepository _imageRepository;

        private readonly IWebHostEnvironment _environment;

        public TripsController(TripRepository tripRepository, 
			UserSettings userSettings, 
			CommentRepository commentRepository,
			ImageRepository imageRepository, 
            IWebHostEnvironment environment)
		{
			_tripRepository = tripRepository;
			_userSettings = userSettings;
			_commentRepository = commentRepository;
			_imageRepository = imageRepository;
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
                Trip trip = new()
                {
                    Name = model.Name,
                    Description = model.Description,
                    Distance = model.Distance,
                    Public = model.Public,
                    StartTimeZoneOffset = model.StartTimeZoneOffset,
                    FinishTimeZoneOffset = model.FinishTimeZoneOffset
                };

                trip.StartTime = model.StartTime.AddSeconds(-model.StartTimeZoneOffset);
                trip.EndTime = model.EndTime.AddSeconds(-model.FinishTimeZoneOffset);

                await UploadImages(trip, images);
				ParseAndAddRoutePoints(trip, routePoints);

				User currentUser = _userSettings.CurrentUser;
				trip.User= currentUser;
				
				await _tripRepository.Add(trip);
				return RedirectToAction(nameof(Index));
			}

            return View(model);
		}

		[Authorize]
		public async Task<IActionResult> Index()
		{
			int userId = _userSettings.CurrentUser.UserId;
			List<Trip> trips = await _tripRepository.GetTripsByUserId(userId);

			List<TripViewModel>? tripModels = trips.Select(t => new TripViewModel(t)).ToList();

			return View(tripModels);
		}

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> History()
		{
			int userId = _userSettings.CurrentUser.UserId;
			List<Trip> trips = await _tripRepository.GetTripsByUserId(userId);

			List<TripViewModel>? tripModels = trips.Where(el => el.EndTime < DateTime.UtcNow)
				.Select(t => new TripViewModel(t)).ToList();

			return View(tripModels);
		}

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> Public()
        {
            int userId = _userSettings.CurrentUser.UserId;
            List<Trip> trips = await _tripRepository.GetOthersPublicTrips(userId);

            List<TripPublicViewModel>? tripModels = trips.Select(t => new TripPublicViewModel(t)).ToList();

            return View(tripModels);
        }

		[Authorize]
		[HttpGet]
        public async Task<IActionResult> Delete(int id)
		{
            Trip tripToDelete = await _tripRepository.GetById(id);
            if (tripToDelete != null)
            {
				DeleteTripImages(tripToDelete);
				await _tripRepository.Delete(tripToDelete);
			}
            return RedirectToAction(nameof(Index));
        }

		[Authorize]
		[HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Trip trip = await _tripRepository.GetById(id);

            if (trip == null)
            {
                return NotFound();
            }

            EditTripViewModel model = new()
            {
                TripId = trip.TripId,
                Name = trip.Name,
                Description = trip.Description,
                StartTimeZoneOffset = trip.StartTimeZoneOffset,
                FinishTimeZoneOffset = trip.FinishTimeZoneOffset,
                Distance = trip.Distance,
                Images = trip.Images,
                RoutePoints = trip.RoutePoints,
                Public = trip.Public,
                StartTime = trip.StartTime.AddSeconds(trip.StartTimeZoneOffset),
                EndTime = trip.EndTime.AddSeconds(trip.FinishTimeZoneOffset)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditTripViewModel model, List<IFormFile> images, string routePoints)
        {
            if (ModelState.IsValid)
            {
                Trip trip = await _tripRepository.GetById(id);
                trip.Description = model.Description;
                trip.Name = model.Name;
                trip.StartTime = model.StartTime.AddSeconds(-model.StartTimeZoneOffset);
                trip.EndTime = model.EndTime.AddSeconds(-model.FinishTimeZoneOffset);
                trip.Distance = model.Distance;
                trip.Public = model.Public;
                trip.StartTimeZoneOffset = model.StartTimeZoneOffset;
                trip.FinishTimeZoneOffset = model.FinishTimeZoneOffset;
                await UploadImages(trip, images);
                
                trip.RoutePoints.Clear();
                ParseAndAddRoutePoints(trip, routePoints);
                await _tripRepository.Update(trip);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> Details(int id)
		{
			Trip trip = await _tripRepository.GetById(id);
			if (trip == null)
			{
				return NotFound();
			}

			var viewModel = new TripDetailsViewModel(trip)
			{
                IsCurrentUserTrip = _userSettings.CurrentUser.UserId == trip.User.UserId
            };

			return View(viewModel);
		}

		[HttpPost]
		public async Task<IActionResult> StartTrip(int id)
		{
			Trip trip = await _tripRepository.GetById(id);
			if (trip == null)
			{
				return NotFound();
			}

			if (trip.StartTime > DateTime.UtcNow)
			{
				trip.EndTime -= trip.StartTime - DateTime.UtcNow;
                trip.EndTime = DateTime.Parse(trip.EndTime.ToString("dd.MM.yyyy HH:mm"));
                trip.StartTime = DateTime.UtcNow;
				trip.StartTime = DateTime.Parse(trip.StartTime.ToString("dd.MM.yyyy HH:mm"));
				await _tripRepository.Update(trip);
			}

			return RedirectToAction(nameof(Details), new { id });
		}

		[HttpPost]
		public async Task<IActionResult> EndTrip(int id)
		{
			Trip trip = await _tripRepository.GetById(id);
			if (trip == null)
			{
				return NotFound();
			}

			if (trip.StartTime < DateTime.UtcNow && trip.EndTime > DateTime.UtcNow)
			{
				trip.EndTime = DateTime.UtcNow;
				trip.EndTime = DateTime.Parse(trip.EndTime.ToString("dd.MM.yyyy HH:mm"));
				await _tripRepository.Update(trip);
			}

			return RedirectToAction(nameof(Details), new { id });
		}

        [HttpPost]
        public async Task<IActionResult> AddComment(NewCommentViewModel model)
		{
            if (ModelState.IsValid)
            {
                User user = _userSettings.CurrentUser;

                Trip trip = await _tripRepository.GetById(model.TripId);

                if (trip != null && user != null)
                {
                    trip.Comments.Add(new Comment
                    {
                        Message = model.Message,
                        TripId = model.TripId,
                        Date = DateTime.UtcNow,
                        User = user
                    });

                    await _tripRepository.Update(trip);
                }
            }

            return RedirectToAction(nameof(Details), new { id = model.TripId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteComment(int tripId, int commentId)
        {
            Comment commentToDelete = await _commentRepository.GetById(commentId);
            if (commentToDelete != null)
            {
                await _commentRepository.Delete(commentToDelete);
            }
            return RedirectToAction(nameof(Details), new { id = tripId });
        }

		private async Task UploadImages(Trip trip, List<IFormFile> images)
		{
            List<string> newImageFileNames = new();
            foreach (var image in images)
            {
                if (image != null && image.Length > 0)
                {
                    string? extension = Path.GetExtension(image.FileName);
                    string newFileName = $"{Guid.NewGuid()}{extension}";
                    string filePath = Path.Combine(_environment.WebRootPath, "images", newFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }
                    newImageFileNames.Add($"/images/{newFileName}");
                }
            }

            foreach (string imageUrl in newImageFileNames)
            {
                trip.Images.Add(new Image { Link = imageUrl });
            }
        }

		private void DeleteTripImages(Trip trip)
		{
            foreach (var image in trip.Images)
                if (System.IO.File.Exists(image.Link))
                    System.IO.File.Delete(image.Link);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            Image image = await _imageRepository.GetById(imageId);

            if (image == null)
            {
                return NotFound();
            }

            string path = _environment.WebRootPath + image.Link;

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                await _imageRepository.Delete(image);
                return NoContent();
            } else
            {
                return NotFound();
            }
        }

        private void ParseAndAddRoutePoints(Trip trip, string routePoints)
        {
            List<RoutePoint> parsedRoutePoints = JsonSerializer.Deserialize<List<RoutePoint>>(routePoints);

            foreach (RoutePoint routePoint in parsedRoutePoints)
            {
                trip.RoutePoints.Add(routePoint);
            }
        }
    }
}
