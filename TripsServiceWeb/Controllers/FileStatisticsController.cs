using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripsServiceBLL.Interfaces;

namespace Andrei_Mikhaleu_Task1.Controllers;

[Route("api/filestats")]
[Authorize(Roles = "Admin")]
[ApiController]
public class FileStatisticsController : ControllerBase
{
    private IFileStatisticsService _fileStatisticsService;

    public FileStatisticsController(IFileStatisticsService fileStatisticsService)
    {
        _fileStatisticsService = fileStatisticsService;
    }

    [HttpGet("tripsTotalDistance")]
    public IActionResult GetTripsTotalDistanceStats()
    {
        byte[] pdfFileBytes = _fileStatisticsService.ExportTripsTotalDistanceDataToPdf();
        return File(pdfFileBytes, "application/pdf", "Trips_total_distance_stats.pdf");
    }
}
