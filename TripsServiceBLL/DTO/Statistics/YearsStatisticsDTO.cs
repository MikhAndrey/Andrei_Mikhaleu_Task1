namespace TripsServiceBLL.DTO.Statistics
{
    public class YearsStatisticsDTO
    {
        public IQueryable<int> Years { get; set; }

        public int SelectedYear;
    }
}
