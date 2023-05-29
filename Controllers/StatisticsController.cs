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
        public async Task<IActionResult> TotalDuration()
        {
            List<int> years = (await _tripRepository
                .GetAllTripsWithUsers(_userSettings.CurrentUser.UserId))
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
        public async Task<IActionResult> TotalDurationData(int year)
        {
            List <DurationInMonth> durations = await _tripRepository
                .GetTotalDurationByMonths(year, _userSettings.CurrentUser.UserId);

            return Json(durations);
        }


        [HttpGet]
        public async Task<IActionResult> HeatMap()
        {
            List<int> years = (await _tripRepository
                .GetAllTripsWithUsers(_userSettings.CurrentUser.UserId))
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
        public async Task<IActionResult> HeatMapData(int year)
        {
            var dataPoints = (await _routePointRepository
                .GetRoutePointsByYear(year, _userSettings.CurrentUser.UserId))
                .Select(rp => new { rp.Latitude, rp.Longitude });

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            return Json(dataPoints, options);
        }
    }
}
