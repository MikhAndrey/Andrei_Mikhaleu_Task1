using Andrei_Mikhaleu_Task1.Common;
using Andrei_Mikhaleu_Task1.Models;
using Andrei_Mikhaleu_Task1.Models.Repos;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Andrei_Mikhaleu_Task1.Controllers
{
    public class StatisticsController : Controller
    {
        
        private RoutePointRepository _routePointRepository;

        private TripRepository _tripRepository;
        
        private UserSettings _userSettings;

        public StatisticsController(RoutePointRepository routePointRepository, TripRepository tripRepository, UserSettings userSettings) 
        {
            _routePointRepository = routePointRepository;
            _tripRepository = tripRepository;
            _userSettings = userSettings;
        }

        [HttpGet]
        public IActionResult HeatMap()
        {
            List<int> years = _tripRepository.GetAllTripsWithUsers()
                .Where(t => t.UserId == _userSettings.CurrentUser.UserId)
                .Select(t => t.StartTime.Year).Distinct().ToList();
            int firstYear = years.FirstOrDefault();
            var viewModel = new HeatmapViewModel() 
            { 
                Years = years,
                SelectedYear= firstYear
            }; 
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult HeatMapData(int year)
        {
            var dataPoints = _routePointRepository.GetRoutePointsByYear(year)
                .Where(rp => rp.Trip.UserId == _userSettings.CurrentUser.UserId)
                .Select(rp => new { rp.Latitude, rp.Longitude });

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            return Json(dataPoints, options);
        }
    }
}
