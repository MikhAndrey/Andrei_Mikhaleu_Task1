using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripsServiceBLL.DTO.RoutePoints;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Utils;

namespace Andrei_Mikhaleu_Task1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StatisticsController : ControllerBase
{
    private readonly IRoutePointService _routePointService;
    private readonly ITripService _tripService;

    public StatisticsController(IRoutePointService routePointService, ITripService tripService)
    {
        _routePointService = routePointService;
        _tripService = tripService;
    }

    [HttpGet("durations")]
    [Authorize]
    public async Task<ActionResult<List<UtilDurationInMonth>>> TotalDurationData(int year)
    {
        List<UtilDurationInMonth> durations = await _tripService.GetTotalDurationByMonthsAsync(year);
        return Ok(durations);
    }

    [HttpGet("heatmap")]
    [Authorize]
    public ActionResult<IEnumerable<RoutePointCoordinatesDTO>> HeatMapData(int year)
    {
        IEnumerable<RoutePointCoordinatesDTO> result = _routePointService.GetRoutePointsByYear(year);
        return Ok(result);
    }

    [HttpGet("years")]
    [Authorize]
    public ActionResult<IEnumerable<int>> GetDistinctYearsModel()
    {
        IEnumerable<int> result = _tripService.GetYearsOfCurrentUserTrips();
        return Ok(result);
    }
}
