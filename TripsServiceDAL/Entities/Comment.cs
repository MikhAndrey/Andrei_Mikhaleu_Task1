using TripsServiceDAL.Interfaces;

namespace TripsServiceDAL.Entities;

public class Comment : IIdentifiable, ISoftDelete
{
	public string? Message { get; set; }
	public DateTime Date { get; set; }
	public int UserId { get; set; }
	public User? User { get; set; }
	public int TripId { get; set; }
	public Trip? Trip { get; set; }
	public int Id { get; set; }
	public bool IsDeleted { get; set; }
}