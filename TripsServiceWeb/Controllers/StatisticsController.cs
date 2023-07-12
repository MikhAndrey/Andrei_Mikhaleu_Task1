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
    public async Task<List<UtilDurationInMonth>> TotalDurationData(int year)
    {
        List<UtilDurationInMonth> durations = await _tripService.GetTotalDurationByMonthsAsync(year);
        return durations;
    }

    [HttpGet("heatmap")]
    [Authorize]
    public IEnumerable<RoutePointCoordinatesDTO> HeatMapData(int year)
    {
        IEnumerable<RoutePointCoordinatesDTO> result = _routePointService.GetRoutePointsByYear(year);
        return result;
    }

    [HttpGet("years")]
    [Authorize]
    public IEnumerable<int> GetDistinctYearsModel()
    {
        IEnumerable<int> result = _tripService.GetYearsOfCurrentUserTrips();
        return result;
    }
}
