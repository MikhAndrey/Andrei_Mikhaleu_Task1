using System.ComponentModel.DataAnnotations;

namespace Andrei_Mikhaleu_Task1.Models
{
	public class TripViewModel
	{
		public int TripId { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		[Display(Name = "Departure Time")]
		public DateTime StartTime { get; set; }

		[Display(Name = "Arrival Time")]
		public DateTime EndTime { get; set; }

		public bool Completed { get; set; }

		public bool IsCurrent { get; set; }

		public bool IsFuture { get; set; }

		public bool IsPast { get; set; }
	}
}
