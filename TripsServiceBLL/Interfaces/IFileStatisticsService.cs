namespace TripsServiceBLL.Interfaces;

public interface IFileStatisticsService
{
    byte[] ExportTripsTotalDistanceDataToPdf();
}
