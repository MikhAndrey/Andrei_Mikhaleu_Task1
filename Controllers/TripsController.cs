using Andrei_Mikhaleu_Task1.Common;
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

		public TripsController(TripRepository tripRepository, UserSettings userSettings)
		{
			_tripRepository = tripRepository;
			_userSettings = userSettings;
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

				List<RoutePoint> parsedRoutePoints = JsonSerializer.Deserialize<List<RoutePoint>>(routePoints);

				foreach (RoutePoint routePoint in parsedRoutePoints)
				{
					trip.RoutePoints.Add(routePoint);
				}

				User currentUser = _userSettings.CurrentUser;
				trip.User= currentUser;
				
				_tripRepository.Add(trip);
				return RedirectToAction("Index", "Home");
			}			
			return View(trip);
		}

		public IActionResult Index()
        {
            return View();
        }
    }
}
