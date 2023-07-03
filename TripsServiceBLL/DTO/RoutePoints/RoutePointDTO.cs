using TripsServiceBLL.DTO.Trips;

namespace TripsServiceBLL.DTO.RoutePoints;

public class RoutePointDTO : RoutePointCoordinatesDTO
{
	public int Id { get; set; }

	public int Ordinal { get; set; }

	public int TripId { get; set; }

	public ReadTripDTO? Trip { get; set; }
}