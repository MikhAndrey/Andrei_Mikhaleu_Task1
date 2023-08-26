using TripsServiceDAL.Interfaces;

namespace TripsServiceDAL.Entities;

public class User : IIdentifiable, ISoftDelete
{
	public User()
	{
		Trips = new List<Trip>();
		Comments = new List<Comment>();
		ChatParticipations = new List<ChatParticipation>();
	}

	public string UserName { get; set; }
	public string Password { get; set; }
	public string? Email { get; set; }
	public List<Trip> Trips { get; set; }
	public List<Comment> Comments { get; set; }
	public List<ChatParticipation> ChatParticipations { get; set; }
	public int Id { get; set; }
	public bool IsDeleted { get; set; }
	public int RoleId { get; set; }
	public Role Role { get; set; }
}
