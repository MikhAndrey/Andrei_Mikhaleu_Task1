using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripsServiceBLL.DTO.RoutePoints;
using TripsServiceBLL.DTO.Statistics;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Utils;

namespace Andrei_Mikhaleu_Task1.Controllers;

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
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> TotalDurationData(int year)
    {
        try
        {
            List<UtilDurationInMonth> durations = await _tripService.GetTotalDurationByMonthsAsync(year);
            return Json(durations);
        }
        catch (ArgumentNullException)
        {
            return RedirectToAction("Login", "Account");
        }
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
        }
    }

    [HttpPost]
    [Authorize]
    public IActionResult HeatMapData(int year)
    {
        try
        {
            IQueryable<RoutePointCoordinatesDTO> result = _routePointService.GetRoutePointsByYear(year);
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };
            return Json(result, options);
        }
        catch (ArgumentNullException)
        {
            return RedirectToAction("Login", "Account");
        }
    }

    private YearsStatisticsDTO GetDistinctYearsModel()
    {
        return _tripService.GetYearsOfCurrentUserTrips();
    }
}