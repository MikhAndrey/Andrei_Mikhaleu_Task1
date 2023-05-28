using Andrei_Mikhaleu_Task1.Common;
using Andrei_Mikhaleu_Task1.Models.ViewModels;
using Andrei_Mikhaleu_Task1.Models.Repos;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using Andrei_Mikhaleu_Task1.Models.Entities.Special;

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
        public IActionResult TotalDuration()
        {
            List<int> years = _tripRepository
                .GetAllTripsWithUsers(_userSettings.CurrentUser.UserId)
                .Select(t => t.StartTime.Year).Distinct().ToList();
            int firstYear = years.FirstOrDefault();
            var viewModel = new YearStatisticsViewModel()
            {
                Years = years,
                SelectedYear = firstYear
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult TotalDurationData(int year)
        {
            List <DurationInMonth> durations = _tripRepository
                .GetTotalDurationByMonths(year, _userSettings.CurrentUser.UserId);

            return Json(durations);
        }


        [HttpGet]
        public IActionResult HeatMap()
        {
            List<int> years = _tripRepository
                .GetAllTripsWithUsers(_userSettings.CurrentUser.UserId)
                .Select(t => t.StartTime.Year).Distinct().ToList();
            int firstYear = years.FirstOrDefault();
            var viewModel = new YearStatisticsViewModel() 
            { 
                Years = years,
                SelectedYear= firstYear
            }; 
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult HeatMapData(int year)
        {
            var dataPoints = _routePointRepository
                .GetRoutePointsByYear(year, _userSettings.CurrentUser.UserId)
                .Select(rp => new { rp.Latitude, rp.Longitude });

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            return Json(dataPoints, options);
        }
    }
}
