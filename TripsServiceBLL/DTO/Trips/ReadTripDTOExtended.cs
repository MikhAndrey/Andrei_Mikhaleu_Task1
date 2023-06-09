using TripsServiceDAL.Entities;

namespace TripsServiceBLL.DTO.Trips
{
	public class ReadTripDTOExtended : ReadTripDTO
	{
		public string UserName { get; set; }

		public ReadTripDTOExtended(Trip trip) : base(trip)
		{
			UserName = trip.User.UserName;
		}
	}
}
