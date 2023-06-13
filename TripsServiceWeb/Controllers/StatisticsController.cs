using Andrei_Mikhaleu_Task1.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using TripsServiceBLL.Commands.RoutePoints;
using TripsServiceBLL.Commands.Statistics;
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
        public async Task<IActionResult> TotalDuration()
        {
            try
            {
                YearsStatisticsDTO model = await GetDistinctYearsModel();
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
            List<DurationInMonth> durations = await new GetTripDurationsByYearCommand(_tripService, userId, year).ExecuteAsync();
            return Json(durations);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> HeatMap()
        {
            try
            {
                YearsStatisticsDTO model = await GetDistinctYearsModel();
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
            IQueryable<RoutePointCoordinatesDTO> result = new GetRoutePointsCoordinatesCommand(_routePointService, userId, year).Execute();
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };
            return Json(result, options);
        }

        private async Task<YearsStatisticsDTO> GetDistinctYearsModel()
        {
            int userId = UserHelper.GetUserIdFromClaims(HttpContext.User.Claims);
            return await new GetDistinctTripYearsCommand(_tripService, userId).ExecuteAsync();
        }
    }
}
