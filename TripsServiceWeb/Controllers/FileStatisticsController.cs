using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TripsServiceBLL.Interfaces;

namespace Andrei_Mikhaleu_Task1.Controllers;

[Route("api/filestats")]
[Authorize(Roles = "Admin")]
[ApiController]
public class FileStatisticsController : ControllerBase
{
    private readonly IFileStatisticsService _fileStatisticsService;
    private readonly IExcelService _excelService;

    public FileStatisticsController(
        IFileStatisticsService fileStatisticsService,
        IExcelService excelService)
    {
        _fileStatisticsService = fileStatisticsService;
        _excelService = excelService;
    }

    [HttpGet("tripsTotalDistance")]
    public IActionResult GetTripsTotalDistanceStats()
    {
        byte[] pdfFileBytes = _fileStatisticsService.ExportTripsTotalDistanceDataToPdf();
        return File(pdfFileBytes, "application/pdf", "Trips_total_distance_stats.pdf");
    }
    
    [HttpGet("convertExcelToJson")]
    public async Task<IActionResult> ConvertExcelToJson()
    {
        await _excelService.ConvertExcelToJsonAsync();
        return Ok();
    }
}
