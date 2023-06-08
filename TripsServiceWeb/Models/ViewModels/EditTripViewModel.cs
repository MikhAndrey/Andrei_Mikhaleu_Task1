using TripsServiceBLL.DTO.Images;
using TripsServiceBLL.DTO.RoutePoints;
using TripsServiceBLL.DTO.Trips;

namespace Andrei_Mikhaleu_Task1.Models.ViewModels
{
    public class EditTripViewModel : NewTripViewModel
    {
        public List<ImageDTO> Images { get; set; }

        public List<RoutePointDTO> RoutePoints { get; set; }

        public int TripId { get; set; }

        public EditTripViewModel(ExtendedExistingTripDTO trip)
        {
            TripId = trip.TripId;
            Name = trip.Name;
            Description = trip.Description;
            StartTimeZoneOffset = trip.StartTimeZoneOffset;
            FinishTimeZoneOffset = trip.FinishTimeZoneOffset;
            Distance = trip.Distance;
            Images = trip.Images;
            RoutePoints = trip.RoutePoints;
            Public = trip.Public;
            StartTime = trip.StartTime.AddSeconds(trip.StartTimeZoneOffset);
            EndTime = trip.EndTime.AddSeconds(trip.FinishTimeZoneOffset);
        }

        public EditTripViewModel() { }
    }
}
