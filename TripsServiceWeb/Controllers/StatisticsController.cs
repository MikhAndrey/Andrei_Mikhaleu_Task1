using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json.Serialization;
using System.Text.Json;
using TripsServiceBLL.Commands.Statistics;
using TripsServiceBLL.Utils;
using TripsServiceBLL.Commands.RoutePoints;
using TripsServiceBLL.DTO.RoutePoints;
using TripsServiceBLL.DTO.Statistics;
using TripsServiceBLL.Interfaces;

namespace Andrei_Mikhaleu_Task1.Controllers
{
	public class StatisticsController : Controller
	{

		private IRoutePointService _routePointService;

		private ITripService _tripService;

		public StatisticsController(IRoutePointService routePointService, ITripService tripService)
		{
			_routePointService = routePointService;
			_tripService = tripService;
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> TotalDuration()
		{
			return View(await GetDistinctYearsModel());
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> TotalDurationData(int year)
		{
			int userId = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value);
			List<DurationInMonth> durations = await new GetTripDurationsByYearCommand(_tripService, userId, year).ExecuteAsync();
			return Json(durations);
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> HeatMap()
		{
			return View(await GetDistinctYearsModel());
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> HeatMapData(int year)
		{
			int userId = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value);
			IQueryable<RoutePointCoordinatesDTO> result = await new GetRoutePointsCoordinatesCommand(_routePointService, userId, year).ExecuteAsync();
			JsonSerializerOptions options = new()
			{
				ReferenceHandler = ReferenceHandler.Preserve
			};
			return Json(result, options);
		}

		private async Task<YearsStatisticsDTO> GetDistinctYearsModel()
		{
			int userId = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value);
			return await new GetDistinctTripYearsCommand(_tripService, userId).ExecuteAsync();
		}
	}
}
