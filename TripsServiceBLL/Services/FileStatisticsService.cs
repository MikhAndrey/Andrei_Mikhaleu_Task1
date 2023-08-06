using System.Globalization;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using TripsServiceBLL.DTO.Trips;
using TripsServiceBLL.Interfaces;

namespace TripsServiceBLL.Services;

public class FileStatisticsService : IFileStatisticsService
{
    private ITripService _tripService;

    public FileStatisticsService(ITripService tripService)
    {
        _tripService = tripService;
    }
    
    public byte[] ExportTripsTotalDistanceDataToPdf()
    {
        IEnumerable<TripsTotalDistanceByUserDTO> data = _tripService.GetTripsTotalDistanceByUser();
        
        CultureInfo currentCulture = CultureInfo.CurrentUICulture;

        MemoryStream stream = new MemoryStream();
        PdfWriter writer = new PdfWriter(stream);
        PdfDocument pdf = new PdfDocument(writer);
        Document document = new Document(pdf);
    
        Table table = new Table(2, true);
        table.AddHeaderCell(new Cell().Add(new Paragraph("User name")));
        table.AddHeaderCell(new Cell().Add(new Paragraph("Total distance of trips for last month")));
    
        foreach (var item in data)
        {
            table.AddCell(new Cell().Add(new Paragraph(item.UserName)));
            table.AddCell(new Cell().Add(new Paragraph(item.Distance.ToString(currentCulture))));
        }
    
        document.Add(table);
        document.Close();

        return stream.ToArray();
    }
}
