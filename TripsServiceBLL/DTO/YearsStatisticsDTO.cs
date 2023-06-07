namespace TripsServiceBLL.DTO
{
    public class YearsStatisticsDTO
    {
        public IQueryable<int> Years { get; set; }

        public int SelectedYear;
    }
}
