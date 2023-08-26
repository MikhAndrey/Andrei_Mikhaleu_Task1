using System.Collections.Concurrent;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using TripsServiceBLL.DTO.Trips;
using TripsServiceBLL.Interfaces;

namespace TripsServiceBLL.Services;

public class ExcelService : IExcelService
{
    private readonly IWebHostEnvironment _hostEnvironment;

    private readonly ITripService _tripService;

    private const string ExcelFileName = "trips_total_distance_by_user.xlsx";
    private const string JsonFileName = "trips_total_distance_by_user.json";
    private const string WorksheetName = "Trips total distance by user";
    private const string UserNameColumnName = "User name";
    private const string TotalDistanceColumnName = "Total distance";


    public ExcelService(IWebHostEnvironment hostEnvironment, ITripService tripService)
    {
        _hostEnvironment = hostEnvironment;
        _tripService = tripService;
    }

    public async Task SaveTripsDataToExcelAsync()
    {
        IEnumerable<TripsTotalDistanceByUserDTO> data = _tripService.GetTripsTotalDistanceByUser();

        using ExcelPackage package = new();
        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(WorksheetName);

        worksheet.Cells[1, 1].Value = UserNameColumnName;
        worksheet.Cells[1, 2].Value = TotalDistanceColumnName;

        int row = 2;
        foreach (TripsTotalDistanceByUserDTO item in data)
        {
            worksheet.Cells[row, 1].Value = item.UserName;
            worksheet.Cells[row, 2].Value = item.Distance;
            row++;
        }

        using MemoryStream stream = new();
        await package.SaveAsAsync(stream);
        stream.Seek(0, SeekOrigin.Begin);

        string filePath = Path.Combine(_hostEnvironment.WebRootPath, ExcelFileName);
        await using FileStream fileStream = new(filePath, FileMode.Create);
        await stream.CopyToAsync(fileStream);
    }

    public async Task ConvertExcelToJsonAsync()
    {
        string filePath = Path.Combine(_hostEnvironment.WebRootPath, ExcelFileName);
        using ExcelPackage package = new(new FileInfo(filePath));

        ExcelWorksheet worksheet = package.Workbook.Worksheets[WorksheetName];
        int rowCount = worksheet.Dimension.Rows;

        string jsonFilePath = Path.Combine(_hostEnvironment.WebRootPath, JsonFileName);

        ConcurrentBag<TripsTotalDistanceByUserDTO> distancesByUser = new();

        await Task.Run(() =>
        {
            Parallel.For(2, rowCount + 1, row =>
            {
                TripsTotalDistanceByUserDTO item = new TripsTotalDistanceByUserDTO
                {
                    UserName = worksheet.Cells[row, 1].GetValue<string>(),
                    Distance = worksheet.Cells[row, 2].GetValue<decimal>()
                };
                distancesByUser.Add(item);
            });
        });

        JsonSerializerOptions options = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        await using FileStream fs = new FileStream(jsonFilePath, FileMode.Create);
        await JsonSerializer.SerializeAsync(fs, distancesByUser, options);
    }
}
