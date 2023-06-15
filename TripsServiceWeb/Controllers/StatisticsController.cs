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
			return View(await GetDistinctYearsModel());
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> TotalDurationData(int year)
		{
			int userId = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == Constants.UserIdClaimName)?.Value);
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
		public IActionResult HeatMapData(int year)
		{
			int userId = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == Constants.UserIdClaimName)?.Value);
			IQueryable<RoutePointCoordinatesDTO> result = new GetRoutePointsCoordinatesCommand(_routePointService, userId, year).Execute();
			JsonSerializerOptions options = new()
			{
				ReferenceHandler = ReferenceHandler.Preserve
			};
			return Json(result, options);
		}

		private async Task<YearsStatisticsDTO> GetDistinctYearsModel()
		{
			int userId = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == Constants.UserIdClaimName)?.Value);
			return await new GetDistinctTripYearsCommand(_tripService, userId).ExecuteAsync();
		}
	}
}
