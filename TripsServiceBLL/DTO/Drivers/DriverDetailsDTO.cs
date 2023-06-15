using TripsServiceBLL.DTO.Feedbacks;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.DTO.Drivers
{
	public class DriverDetailsDTO
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public List<DriverPhoto> Photos { get; set; }

		public int Experience { get; set; }

		public double AverageRating { get; set; }

		public IEnumerable<ReadFeedbackDTO> Feedbacks { get; set; }
	}
}
