namespace TripsServiceDAL.Entities
{
	public class DriverPhoto
	{
		public DriverPhoto()
		{
			Driver = new();
		}

		public int Id { get; set; }

		public string Link { get; set; }

		public int DriverId { get; set; }

		public Driver Driver { get; set; }
	}
}
