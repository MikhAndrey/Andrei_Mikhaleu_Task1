using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Utils
{
    public static class DriverUtils
    {
        public static double ComputeAverageRating(Driver driver)
        {
            return Math.Round(driver.Trips.Where(t => t.Feedback != null)
               .Average(t => (double?)t.Feedback.Rating) ?? 0, 1);
        }
    }
}
