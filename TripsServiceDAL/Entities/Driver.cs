using System.ComponentModel.DataAnnotations.Schema;

namespace TripsServiceDAL.Entities
{
	public class Driver
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public List<DriverPhoto> Photos { get; set; }

		public int Experience { get; set; }

		public List<Trip> Trips { get; set; }
	}
}
