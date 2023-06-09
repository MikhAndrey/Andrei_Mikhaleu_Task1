using TripsServiceBLL.DTO.Comments;
using TripsServiceBLL.DTO.Images;
using TripsServiceBLL.DTO.RoutePoints;
using TripsServiceBLL.Infrastructure;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.DTO.Trips
{
	public class TripDetailsDTO : ReadTripDTO
	{
		public bool IsCurrentUserTrip { get; set; } 

		public bool Public { get; set; }

		public List<ImageDTO> Images { get; set; }

		public List<RoutePointDTO> RoutePoints { get; set; }

		public List<CommentDTO> Comments { get; set; }

		public int StartTimeZoneOffset { get; set; }

		public int FinishTimeZoneOffset { get; set; }

		public decimal Distance { get; set; }

		public TripDetailsDTO(Trip trip, int userId) : base(trip) 
		{
			IsCurrentUserTrip = trip.User.UserId == userId;
			Public = trip.Public;
			Images = CustomMapper<List<Image>, List<ImageDTO>>.Map(trip.Images);
			RoutePoints = CustomMapper<List<RoutePoint>, List<RoutePointDTO>>.Map(trip.RoutePoints);
			Comments = CustomMapper<List<Comment>, List<CommentDTO>>.Map(trip.Comments);
			StartTimeZoneOffset = trip.StartTimeZoneOffset;
			FinishTimeZoneOffset = trip.FinishTimeZoneOffset;
			Distance = trip.Distance;
		}

		public TripDetailsDTO() { }
	}
}
