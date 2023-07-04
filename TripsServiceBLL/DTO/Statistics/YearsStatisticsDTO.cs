namespace TripsServiceBLL.DTO.Statistics;

public class YearsStatisticsDTO
{
	public int SelectedYear;
	public IQueryable<int>? Years { get; set; }
}
