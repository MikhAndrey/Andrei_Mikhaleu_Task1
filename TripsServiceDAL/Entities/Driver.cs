namespace TripsServiceDAL.Entities
{
	public class Driver
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public List<Trip> Trips { get; set; }
	}
}
