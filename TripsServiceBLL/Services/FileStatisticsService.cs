using System.Globalization;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
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
    
        Style headerStyle = new Style()
            .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
            .SetFontSize(20)
            .SetTextAlignment(TextAlignment.CENTER);

        Style tableHeaderStyle = new Style()
            .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
            .SetFontSize(12)
            .SetBackgroundColor(ColorConstants.ORANGE)
            .SetTextAlignment(TextAlignment.CENTER);
        
        Style tableCellStyle = new Style()
            .SetBackgroundColor(ColorConstants.LIGHT_GRAY);

        Paragraph paragraph = new Paragraph("Last month trips total distance by user").AddStyle(headerStyle);
        document.Add(paragraph);
        
        Table table = new Table(2, true);
        table.AddHeaderCell(new Cell().Add(new Paragraph("User name")).AddStyle(tableHeaderStyle));
        table.AddHeaderCell(new Cell().Add(new Paragraph("Total distance")).AddStyle(tableHeaderStyle));
    
        foreach (var item in data)
        {
            table.AddCell(new Cell().Add(new Paragraph(item.UserName))).AddStyle(tableCellStyle);
            table.AddCell(new Cell().Add(new Paragraph(item.Distance.ToString(currentCulture)))).AddStyle(tableCellStyle);
        }
    
        document.Add(table);
        document.Close();

        return stream.ToArray();
    }
}
