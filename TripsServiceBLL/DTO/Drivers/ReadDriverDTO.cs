namespace TripsServiceBLL.DTO.Drivers;

public class ReadDriverDTO
{
	public int Id { get; set; }
	public string? Name { get; set; }
	public string? PhotoLink { get; set; }
	public int Experience { get; set; }
	public double AverageRating { get; set; }
}
