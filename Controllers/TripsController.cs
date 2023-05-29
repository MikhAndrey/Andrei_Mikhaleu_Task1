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

        public TripsController(TripRepository tripRepository, 
			UserSettings userSettings, 
			CommentRepository commentRepository,
			ImageRepository imageRepository)
		{
			_tripRepository = tripRepository;
			_userSettings = userSettings;
			_commentRepository = commentRepository;
			_imageRepository = imageRepository;
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
				
				_tripRepository.Add(trip);
				return RedirectToAction(nameof(Index));
			}

            return View(model);
		}

		[Authorize]
		public IActionResult Index()
		{
			var userId = _userSettings.CurrentUser.UserId;
			var trips = _tripRepository.GetTripsByUserId(userId);

			var tripModels = trips.Select(t => new TripViewModel(t)).ToList();

			return View(tripModels);
		}

		[Authorize]
		[HttpGet]
		public IActionResult History()
		{
			var userId = _userSettings.CurrentUser.UserId;
			var trips = _tripRepository.GetTripsByUserId(userId);

			var tripModels = trips.Where(el => el.EndTime < DateTime.UtcNow)
				.Select(t => new TripViewModel(t)).ToList();

			return View(tripModels);
		}

		[Authorize]
		[HttpGet]
		public IActionResult Public()
        {
            var userId = _userSettings.CurrentUser.UserId;
            var trips = _tripRepository.GetOthersPublicTrips(userId);

            var tripModels = trips.Select(t => new TripPublicViewModel(t)).ToList();

            return View(tripModels);
        }

		[Authorize]
		[HttpGet]
        public IActionResult Delete(int id)
		{
            Trip tripToDelete = _tripRepository.GetById(id);
            if (tripToDelete != null)
            {
				DeleteTripImages(tripToDelete);
				_tripRepository.Delete(tripToDelete);
			}
            return RedirectToAction("Index");
        }

		[Authorize]
		[HttpGet]
        public IActionResult Edit(int id)
        {
            var trip = _tripRepository.GetById(id);

            if (trip == null)
            {
                return NotFound();
            }
			trip.StartTime = trip.StartTime.AddSeconds(trip.StartTimeZoneOffset);
			trip.EndTime = trip.EndTime.AddSeconds(trip.FinishTimeZoneOffset);

            return View(trip);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name", "StartTime", "EndTime", "Public", "Description", "Distance", "StartTimeZoneOffset", "FinishTimeZoneOffset")] Trip t, List<IFormFile> images, string routePoints)
        {
            Trip trip = _tripRepository.GetById(id);
            trip.Description = t.Description;
            trip.Name = t.Name;
            trip.StartTime = t.StartTime.AddSeconds(-t.StartTimeZoneOffset);
            trip.EndTime = t.EndTime.AddSeconds(-t.FinishTimeZoneOffset);
            trip.Distance = t.Distance;
            trip.Public = t.Public;
			trip.StartTimeZoneOffset = t.StartTimeZoneOffset;
			trip.FinishTimeZoneOffset = t.FinishTimeZoneOffset;

            if (ModelState.IsValid)
            {
                await UploadImages(trip, images);
                
                trip.RoutePoints.Clear();
                ParseAndAddRoutePoints(trip, routePoints);
                _tripRepository.Update(trip);
                return RedirectToAction(nameof(Index));
            }

            return View(trip);
        }

		[Authorize]
		[HttpGet]
		public IActionResult Details(int id)
		{
			var trip = _tripRepository.GetById(id);
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
		public IActionResult StartTrip(int id)
		{
			var trip = _tripRepository.GetById(id);
			if (trip == null)
			{
				return NotFound();
			}

			if (trip.StartTime > DateTime.UtcNow)
			{
				trip.EndTime -= trip.StartTime - DateTime.UtcNow;
                trip.EndTime.AddMilliseconds(-trip.StartTime.Millisecond);
                trip.EndTime.AddSeconds(-trip.StartTime.Second);
                trip.StartTime = DateTime.UtcNow;
				trip.StartTime.AddMilliseconds(-trip.StartTime.Millisecond);
                trip.StartTime.AddSeconds(-trip.StartTime.Second);
                _tripRepository.Update(trip);
			}

			return RedirectToAction(nameof(Details), new { id });
		}

		[HttpPost]
		public IActionResult EndTrip(int id)
		{
			var trip = _tripRepository.GetById(id);
			if (trip == null)
			{
				return NotFound();
			}

			if (trip.StartTime < DateTime.UtcNow && trip.EndTime > DateTime.UtcNow)
			{
				trip.EndTime = DateTime.UtcNow;
				_tripRepository.Update(trip);
			}

			return RedirectToAction(nameof(Details), new { id });
		}

        [HttpPost]
        public IActionResult AddComment(int tripId, string comment)
		{
			var user = _userSettings.CurrentUser;

			var trip = _tripRepository.GetById(tripId);

            if (trip != null && user != null)
            {
                trip.Comments.Add(new Comment
                {
                    User = user,
					Message = comment,
                    Date = DateTime.UtcNow
                });

                _tripRepository.Update(trip);
            }

            return RedirectToAction(nameof(Details), new { id = tripId });
        }

        [HttpPost]
        public IActionResult DeleteComment(int tripId, int commentId)
        {
            Comment commentToDelete = _commentRepository.GetById(commentId);
            if (commentToDelete != null)
            {
                _commentRepository.Delete(commentToDelete);
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
                    var extension = Path.GetExtension(image.FileName);
                    var newFileName = $"{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", newFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }
                    newImageFileNames.Add($"/images/{newFileName}");
                }
            }

            foreach (var imageUrl in newImageFileNames)
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
        public IActionResult DeleteImage(int imageId, int tripId)
        {
            Image image = _imageRepository.GetById(imageId);

            if (image == null)
            {
                return NotFound();
            }

            string path = "wwwroot" + image.Link;

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                _imageRepository.Delete(image);
                return RedirectToAction(nameof(Edit), new { id = tripId});
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
