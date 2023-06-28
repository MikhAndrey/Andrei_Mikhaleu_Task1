using TripsServiceDAL.Interfaces;

namespace TripsServiceDAL.Entities
{
	public class User : IIdentifiable
	{
		public User()
		{
			Trips = new();
			Comments = new();
		}

		public int Id { get; set; }

		public string UserName { get; set; }

		public string Password { get; set; }

		public string? Email { get; set; }

		public List<Trip> Trips { get; set; }

		public List<Comment> Comments { get; set; }

	}
}
