using TripsServiceBLL.DTO.Feedbacks;
using TripsServiceBLL.DTO.Images;
using TripsServiceBLL.Interfaces;

namespace TripsServiceBLL.DTO.Drivers;

public class DriverDetailsDTO : IHasImages
{
	public int Id { get; set; }
	public string Name { get; set; }
	public List<ImageDTO> Images { get; set; }
	public int Experience { get; set; }
	public double AverageRating { get; set; }
	public IEnumerable<ReadFeedbackDTO> Feedbacks { get; set; }
}
