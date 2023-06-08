using Andrei_Mikhaleu_Task1.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using TripsServiceBLL.Services;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json.Serialization;
using System.Text.Json;
using TripsServiceBLL.Commands.Statistics;
using TripsServiceBLL.Utils;
using TripsServiceBLL.Commands.RoutePoints;
using TripsServiceBLL.DTO.RoutePoints;
using TripsServiceBLL.DTO.Statistics;

namespace Andrei_Mikhaleu_Task1.Controllers
{
    public class StatisticsController : Controller
    {

        private RoutePointService _routePointService;

        private TripService _tripService;

        private UserService _userService;

        public StatisticsController(RoutePointService routePointService, TripService tripService, UserService userService)
        {
            _routePointService = routePointService;
            _tripService = tripService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> TotalDuration()
        {
			return View(await GetDistinctYearsModel());
		}

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> TotalDurationData(int year)
        {
            string? userName = HttpContext?.User?.Identity?.Name;
            List<DurationInMonth> durations = await new GetTripDurationsByYearCommand(_tripService, _userService, userName, year).ExecuteAsync();
            return Json(durations);
        }

        [HttpGet]
        public async Task<IActionResult> HeatMap()
        {
            return View(await GetDistinctYearsModel());
        }

        [HttpPost]
        public async Task<IActionResult> HeatMapData(int year)
        {
            string? userName = HttpContext?.User?.Identity?.Name;
            IQueryable<RoutePointCoordinatesDTO> result = await new GetRoutePointsCoordinatesCommand(_routePointService, _userService, userName, year).ExecuteAsync();
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };
            return Json(result, options);
        }

        private async Task<YearStatisticsViewModel> GetDistinctYearsModel()
        {
            string? userName = HttpContext?.User?.Identity?.Name;
			YearsStatisticsDTO yearsInfo =  await new GetDistinctTripYearsCommand(_tripService, _userService, userName).ExecuteAsync();
            return new()
            {
                Years = yearsInfo.Years,
                SelectedYear = yearsInfo.SelectedYear
            };
        }
    }
}
