namespace TripsServiceBLL.Interfaces;

public interface IExcelService
{
    Task SaveTripsDataToExcelAsync();
    Task ConvertExcelToJsonAsync();
}
