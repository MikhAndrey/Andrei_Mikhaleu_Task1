using TripsServiceBLL.DTO.Trips;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Utils;

public static class UtilTripFunctions
{
	public static bool IsTripPastAndHasFeedback(Trip trip, ReadTripDTO dto)
	{
		return trip.DriverId != null && trip.Feedback == null && dto.IsPast;
	}

	public static Func<Trip, bool> IsPast = trip => DateTime.UtcNow > trip.EndTime;

	public static Func<Trip, bool> IsCurrent = trip =>
		DateTime.UtcNow >= trip.StartTime && DateTime.UtcNow <= trip.EndTime;

	public static Func<Trip, bool> IsFuture = trip => DateTime.UtcNow < trip.StartTime;
}