using Andrei_Mikhaleu_Task1.Models.Entities.Common;

namespace Andrei_Mikhaleu_Task1.Models.ViewModels
{
    public class TripViewModel
    {
        public int TripId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public bool IsCurrent { get; set; }

        public bool IsFuture { get; set; }

        public bool IsPast { get; set; }

        public string UtcStartTimeZone { get; set; }

        public string UtcFinishTimeZone { get; set; }

        public TripViewModel(Trip trip)
        {
            TripId = trip.TripId;
            Name = trip.Name;
            Description = trip.Description;
            StartTime = trip.StartTime.AddSeconds(trip.StartTimeZoneOffset);
            EndTime = trip.EndTime.AddSeconds(trip.FinishTimeZoneOffset);
            IsCurrent = DateTime.UtcNow >= trip.StartTime && DateTime.UtcNow <= trip.EndTime;
            IsFuture = DateTime.UtcNow < trip.StartTime;
            IsPast = DateTime.UtcNow > trip.EndTime;
            UtcStartTimeZone = string.Concat(StartTime.ToString("dd.MM.yyyy HH:mm"), $" UTC{trip.StartTimeZoneOffset / 3600:+#;-#;+0}");
            UtcFinishTimeZone = string.Concat(EndTime.ToString("dd.MM.yyyy HH:mm"), $" UTC{trip.FinishTimeZoneOffset / 3600:+#;-#;+0}");
        }

    }
}
