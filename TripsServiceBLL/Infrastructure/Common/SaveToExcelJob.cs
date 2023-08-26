using Quartz;
using TripsServiceBLL.Interfaces;

namespace TripsServiceBLL.Infrastructure.Common;

public class SaveToExcelJob : IJob
{
    private readonly IExcelService _excelService;

    public SaveToExcelJob(IExcelService excelService)
    {
        _excelService = excelService;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        await _excelService.SaveTripsDataToExcelAsync();
    }
}
