using Andrei_Mikhaleu_Task1.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using TripsServiceBLL.Services;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json.Serialization;
using System.Text.Json;
using TripsServiceBLL.DTO;
using TripsServiceBLL.Infrastructure;
using TripsServiceBLL.Commands;
using TripsServiceBLL.Commands.Statistics;
using TripsServiceBLL.Utils;
using TripsServiceBLL.Commands.RoutePoints;

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
            try {
                YearStatisticsViewModel viewModel = await GetDistinctYearsModel();
                return View(viewModel);
            } catch (ValidationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> TotalDurationData(int year)
        {
            string? userName = HttpContext?.User?.Identity?.Name;
            AsyncGenericCommandInvoker<List<DurationInMonth>> invoker = new()
            {
                Command = new GetTripDurationsByYearCommand(_tripService, _userService, userName, year)
            };
            try
            {
                List<DurationInMonth> durations = await invoker.ExecuteCommandAsync();
                return Json(durations);
            }
            catch (ValidationException ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpGet]
        public async Task<IActionResult> HeatMap()
        {
            try
            {
                YearStatisticsViewModel viewModel = await GetDistinctYearsModel();
                return View(viewModel);
            }
            catch (ValidationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> HeatMapData(int year)
        {
            string? userName = HttpContext?.User?.Identity?.Name;
            AsyncGenericCommandInvoker<IQueryable<RoutePointCoordinatesDTO>> invoker = new()
            {
                Command = new GetRoutePointsCoordinatesCommand(_routePointService, _userService, userName, year)
            };
            try
            {
                IQueryable<RoutePointCoordinatesDTO> result = await invoker.ExecuteCommandAsync();
                JsonSerializerOptions options = new()
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                };
                return Json(result, options);
            } catch (ValidationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        private async Task<YearStatisticsViewModel> GetDistinctYearsModel()
        {
            string? userName = HttpContext?.User?.Identity?.Name;

            AsyncGenericCommandInvoker<YearsStatisticsDTO> invoker = new()
            {
                Command = new GetDistinctTripYearsCommand(_tripService, _userService, userName)
            };
            
            YearsStatisticsDTO yearsInfo = await invoker.ExecuteCommandAsync();
            return new()
            {
                Years = yearsInfo.Years,
                SelectedYear = yearsInfo.SelectedYear
            };
        }
    }
}
