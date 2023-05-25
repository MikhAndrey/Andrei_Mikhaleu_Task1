using Andrei_Mikhaleu_Task1.Common;
using Andrei_Mikhaleu_Task1.Models;
using Andrei_Mikhaleu_Task1.Models.Entities;
using Andrei_Mikhaleu_Task1.Models.Repos;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Andrei_Mikhaleu_Task1.Controllers
{
    public class TripsController : Controller
    {

        private readonly TripRepository _tripRepository;

		private readonly UserSettings _userSettings;

		private readonly CommentRepository _commentRepository;

		public TripsController(TripRepository tripRepository, UserSettings userSettings, CommentRepository commentRepository)
		{
			_tripRepository = tripRepository;
			_userSettings = userSettings;
			_commentRepository = commentRepository;
		}

		[HttpGet]
        public IActionResult Create()
        {
			return View();
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Name","StartTime","EndTime","Public","Description", "Distance")] Trip trip, List<IFormFile> images, string routePoints)
		{
			if (ModelState.IsValid)
			{
				UploadImages(trip, images);
				ParseAndAddRoutePoints(trip, routePoints);

				User currentUser = _userSettings.CurrentUser;
				trip.User= currentUser;
				
				_tripRepository.Add(trip);
				return RedirectToAction(nameof(Index));
			}			
			return View(trip);
		}

		public IActionResult Index()
		{
			var userId = _userSettings.CurrentUser.UserId;
			var trips = _tripRepository.GetTripsByUserId(userId);

			var tripModels = trips.Select(t => new TripViewModel
			{
				TripId = t.TripId,
				Name = t.Name,
				Description = t.Description,
				StartTime = t.StartTime,
				EndTime = t.EndTime,
				Completed = t.Completed,
				IsCurrent = DateTime.Now >= t.StartTime && DateTime.Now <= t.EndTime,
				IsFuture = DateTime.Now < t.StartTime,
				IsPast = DateTime.Now > t.EndTime
			}).ToList();

			return View(tripModels);
		}

		[HttpGet]
		public IActionResult History()
		{
			var userId = _userSettings.CurrentUser.UserId;
			var trips = _tripRepository.GetTripsByUserId(userId);

			var tripModels = trips.Where(el => el.EndTime < DateTime.Now).Select(t => new TripViewModel
			{
				TripId = t.TripId,
				Name = t.Name,
				Description = t.Description,
				StartTime = t.StartTime,
				EndTime = t.EndTime,
				Completed = t.Completed,
				IsCurrent = DateTime.Now >= t.StartTime && DateTime.Now <= t.EndTime,
				IsFuture = DateTime.Now < t.StartTime,
				IsPast = DateTime.Now > t.EndTime
			}).ToList();

			return View(tripModels);
		}

		[HttpGet]
		public IActionResult Public()
        {
            var userId = _userSettings.CurrentUser.UserId;
            var trips = _tripRepository.GetOthersPublicTrips(userId);

            var tripModels = trips.Select(t => new TripPublicViewModel
            {
                TripId = t.TripId,
                Name = t.Name,
				UserName = t.User.UserName,
                Description = t.Description,
                StartTime = t.StartTime,
                EndTime = t.EndTime,
                Completed = t.Completed,
                IsCurrent = DateTime.Now >= t.StartTime && DateTime.Now <= t.EndTime,
                IsFuture = DateTime.Now < t.StartTime,
                IsPast = DateTime.Now > t.EndTime
            }).ToList();

            return View(tripModels);
        }

        [HttpGet]
        public IActionResult Delete(int id)
		{
            Trip tripToDelete = _tripRepository.GetById(id);
            if (tripToDelete != null)
            {
				_tripRepository.Delete(tripToDelete);
			}
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var trip = _tripRepository.GetById(id);

            if (trip == null)
            {
                return NotFound();
            }

            return View(trip);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TripId", "Name", "StartTime", "EndTime", "Public", "Description", "Distance")] Trip t, List<IFormFile> images, string routePoints)
        {
            Trip trip = _tripRepository.GetById(id);
            trip.Description = t.Description;
            trip.Name = t.Name;
            trip.StartTime = t.StartTime;
            trip.EndTime = t.EndTime;
            trip.Distance = t.Distance;
            trip.Public = t.Public;

            if (ModelState.IsValid)
            {
				foreach (var image in trip.Images)
					if (System.IO.File.Exists(image.Link))
						System.IO.File.Delete(image.Link);
				
                trip.Images.Clear();
                await UploadImages(trip, images);
                
                trip.RoutePoints.Clear();
                ParseAndAddRoutePoints(trip, routePoints);
                _tripRepository.Update(trip);
                return RedirectToAction(nameof(Index), new { id = trip.TripId });
            }

            return View(trip);
        }

        public IActionResult Details(int id)
		{
			var trip = _tripRepository.GetById(id);
			if (trip == null)
			{
				return NotFound();
			}
			var viewModel = new TripDetailsViewModel
			{
				Trip= trip,
				IsCurrent = DateTime.Now >= trip.StartTime && DateTime.Now <= trip.EndTime,
				IsFuture = DateTime.Now < trip.StartTime,
				IsPast = DateTime.Now > trip.EndTime,
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

			if (trip.StartTime > DateTime.Now)
			{
				trip.EndTime -= trip.StartTime - DateTime.Now;
                trip.EndTime.AddMilliseconds(-trip.StartTime.Millisecond);
                trip.EndTime.AddSeconds(-trip.StartTime.Second);
                trip.StartTime = DateTime.Now;
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

			if (trip.StartTime < DateTime.Now && trip.EndTime > DateTime.Now)
			{
				trip.EndTime = DateTime.Now;
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
