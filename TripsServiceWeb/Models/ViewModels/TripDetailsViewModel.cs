using TripsServiceBLL.DTO;

namespace Andrei_Mikhaleu_Task1.Models.ViewModels
{
    public class TripDetailsViewModel
    {
        public bool IsCurrentUserTrip { get; set; }

        public TripDTO Trip { get; set; }

        public bool IsCurrent { get; set; }

        public bool IsFuture { get; set; }

        public bool IsPast { get; set; }

        public string UtcStartTimeZone { get; set; }

        public string UtcFinishTimeZone { get; set; }

        public TripDetailsViewModel(TripDTO trip)
        {
            Trip = trip;
            IsCurrentUserTrip = trip.IsCurrentUserTrip;
            IsCurrent = DateTime.UtcNow >= trip.StartTime && DateTime.UtcNow <= trip.EndTime;
            IsFuture = DateTime.UtcNow < trip.StartTime;
            IsPast = DateTime.UtcNow > trip.EndTime;
            Trip.StartTime = Trip.StartTime.AddSeconds(Trip.StartTimeZoneOffset);
            Trip.EndTime = Trip.EndTime.AddSeconds(Trip.FinishTimeZoneOffset);
            UtcStartTimeZone = string.Concat(Trip.StartTime.ToString("dd.MM.yyyy HH:mm"), $" UTC{trip.StartTimeZoneOffset / 3600:+#;-#;+0}");
            UtcFinishTimeZone = string.Concat(Trip.EndTime.ToString("dd.MM.yyyy HH:mm"), $" UTC{trip.FinishTimeZoneOffset / 3600:+#;-#;+0}");
        }
    }
}
