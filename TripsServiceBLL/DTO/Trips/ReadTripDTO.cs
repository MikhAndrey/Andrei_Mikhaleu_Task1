namespace TripsServiceBLL.DTO.Trips;

public class ReadTripDTO
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string? Description { get; set; }
	public DateTime StartTime { get; set; }
	public DateTime EndTime { get; set; }
	public bool IsCurrent { get; set; }
	public bool IsFuture { get; set; }
	public bool IsPast { get; set; }
	public string UtcStartTimeZone { get; set; }
	public string UtcFinishTimeZone { get; set; }
	public int? Rating { get; set; }
	public bool IsNeedToBeRated { get; set; }
}
