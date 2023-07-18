using TripsServiceBLL.DTO.Trips;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Utils;

public static class UtilTripFunctions
{
	public static bool IsTripPastAndHasFeedback(Trip trip, ReadTripDTO dto)
	{
		return trip.DriverId != null && trip.Feedback == null && dto.IsPast;
	}
}