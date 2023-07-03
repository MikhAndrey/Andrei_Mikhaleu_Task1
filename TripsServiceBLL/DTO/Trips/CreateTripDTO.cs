using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using TripsServiceBLL.Interfaces;

namespace TripsServiceBLL.DTO.Trips;

public class CreateTripDTO : IMinimalTripFactory
{
	[Required(ErrorMessage = "Please select the start time of your trip")]
	public DateTime StartTime { get; set; }

	[Required(ErrorMessage = "This field will be filled automatically when you build the route of at least two points")]
	public DateTime EndTime { get; set; }

	public decimal Distance { get; set; }

	public int StartTimeZoneOffset { get; set; }

	public int FinishTimeZoneOffset { get; set; }

	public int? DriverId { get; set; }

	public string RoutePointsAsString { get; set; }

	public List<IFormFile>? ImagesAsFiles { get; set; }

	[Required(ErrorMessage = "Please enter a name for your trip")]
	public string? Name { get; set; }

	public bool Public { get; set; }

	public string? Description { get; set; }
}