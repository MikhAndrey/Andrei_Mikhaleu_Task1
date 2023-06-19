using Andrei_Mikhaleu_Task1.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using TripsServiceBLL.DTO.RoutePoints;
using TripsServiceBLL.DTO.Statistics;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Utils;

namespace Andrei_Mikhaleu_Task1.Controllers
{
    public class StatisticsController : Controller
    {

        private readonly IRoutePointService _routePointService;

        private readonly ITripService _tripService;

        public StatisticsController(IRoutePointService routePointService, ITripService tripService)
        {
            _routePointService = routePointService;
            _tripService = tripService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult TotalDuration()
        {
            try
            {
                YearsStatisticsDTO model = GetDistinctYearsModel();
                return View(model);
            }
            catch (ArgumentNullException)
            {
                return RedirectToAction("Login", "Account");
            };
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> TotalDurationData(int year)
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
            List<DurationInMonth> durations = await _tripService.GetTotalDurationByMonthsAsync(year, userId);
            return Json(durations);
        }

        [HttpGet]
        [Authorize]
        public IActionResult HeatMap()
        {
            try
            {
                YearsStatisticsDTO model = GetDistinctYearsModel();
                return View(model);
            }
            catch (ArgumentNullException)
            {
                return RedirectToAction("Login", "Account");
            };
        }

        [HttpPost]
        [Authorize]
        public IActionResult HeatMapData(int year)
        {
            int userId;
            try
            {
                userId = UserHelper.GetUserIdFromClaims(HttpContext.User.Claims);
            }
            catch (ArgumentNullException)
            {
                return RedirectToAction("Login", "Account");
            };
            IQueryable<RoutePointCoordinatesDTO> result = _routePointService.GetRoutePointsByYear(year, userId);
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };
            return Json(result, options);
        }

        private YearsStatisticsDTO GetDistinctYearsModel()
        {
            int userId = UserHelper.GetUserIdFromClaims(HttpContext.User.Claims);
            return _tripService.GetYearsOfUserTrips(userId);
        }
    }
}