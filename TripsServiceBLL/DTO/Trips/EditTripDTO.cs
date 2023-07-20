using TripsServiceBLL.DTO.Images;
using TripsServiceBLL.DTO.RoutePoints;
using TripsServiceBLL.Interfaces;

namespace TripsServiceBLL.DTO.Trips;

public class EditTripDTO : CreateTripDTO, IMinimalTripChanges, IHasImages
{
	public List<RoutePointDTO>? RoutePoints { get; set; }
	public List<ImageDTO>? Images { get; set; }
	public int Id { get; set; }
	public int UserId { get; set; }
}
