namespace Andrei_Mikhaleu_Task1.Models.ViewModels
{
    public class YearStatisticsViewModel
    {
        public IQueryable<int> Years { get; set; }
        public int SelectedYear { get; set; }
    }
}
