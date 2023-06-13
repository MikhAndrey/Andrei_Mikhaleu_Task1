using TripsServiceBLL.DTO.Images;
using TripsServiceBLL.DTO.RoutePoints;
using TripsServiceBLL.Infrastructure;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.DTO.Trips
{
	public class EditTripDTO : CreateTripDTO
	{
		public List<ImageDTO> Images { get; set; }

		public List<RoutePointDTO> RoutePoints { get; set; }

		public int TripId { get; set; }

		public int UserId { get; set; }

		public EditTripDTO(Trip trip)
		{
			TripId = trip.TripId;
			UserId = trip.UserId;
			Name = trip.Name;
			Description = trip.Description;
			StartTimeZoneOffset = trip.StartTimeZoneOffset;
			FinishTimeZoneOffset = trip.FinishTimeZoneOffset;
			Distance = trip.Distance;
			Images = CustomMapper<List<Image>, List<ImageDTO>>.Map(trip.Images);
			RoutePoints = CustomMapper<List<RoutePoint>, List<RoutePointDTO>>.Map(trip.RoutePoints);
			Public = trip.Public;
			StartTime = trip.StartTime.AddSeconds(trip.StartTimeZoneOffset);
			EndTime = trip.EndTime.AddSeconds(trip.FinishTimeZoneOffset);
		}

		public EditTripDTO() { }
	}
}
