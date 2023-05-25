using Andrei_Mikhaleu_Task1.Models.Entities;

namespace Andrei_Mikhaleu_Task1.Models
{
	public class TripDetailsViewModel
	{
		public bool IsCurrentUserTrip { get; set;}

		public Trip Trip { get; set;}

        public bool IsCurrent { get; set; }

        public bool IsFuture { get; set; }

        public bool IsPast { get; set; }
    }
}
