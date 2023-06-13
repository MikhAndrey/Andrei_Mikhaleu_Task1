namespace TripsServiceDAL.Entities
{
	public class Image
	{
		public Image()
		{
			Trip = new();
		}

		public int ImageId { get; set; }

		public string Link { get; set; }

		public int TripId { get; set; }

		public Trip Trip { get; set; }

	}
}
