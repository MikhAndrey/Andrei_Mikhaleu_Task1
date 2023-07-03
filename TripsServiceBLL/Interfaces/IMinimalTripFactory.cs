namespace TripsServiceBLL.Interfaces;

public interface IMinimalTripFactory
{
	public string? Name { get; set; }

	public bool Public { get; set; }

	public string? Description { get; set; }
}