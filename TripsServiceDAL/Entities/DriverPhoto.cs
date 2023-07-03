using TripsServiceDAL.Interfaces;

namespace TripsServiceDAL.Entities;

public class DriverPhoto : IIdentifiable, ISoftDelete
{
	public string? Link { get; set; }
	public int DriverId { get; set; }
	public Driver? Driver { get; set; }
	public int Id { get; set; }
	public bool IsDeleted { get; set; }
}