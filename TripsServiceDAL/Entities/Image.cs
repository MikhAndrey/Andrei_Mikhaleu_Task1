using TripsServiceDAL.Interfaces;

namespace TripsServiceDAL.Entities
{
	public class Image : IIdentifiable, ISoftDelete
	{
		public int Id { get; set; }

		public string? Link { get; set; }

		public int TripId { get; set; }

		public Trip? Trip { get; set; }

		public bool IsDeleted { get; set; }
	}
}
