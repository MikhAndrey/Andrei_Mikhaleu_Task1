using TripsServiceBLL.DTO.Images;

namespace TripsServiceBLL.Interfaces;

public interface IMinimalTripChanges
{
	public List<ImageDTO>? Images { get; set; }
	public int Id { get; set; }
}
