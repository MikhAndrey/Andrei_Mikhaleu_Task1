using Andrei_Mikhaleu_Task1.Models.Entities.Common;

namespace Andrei_Mikhaleu_Task1.Models.ViewModels
{
	public class EditTripViewModel : NewTripViewModel
	{
		public List<Image> Images { get; set; }

		public List<RoutePoint> RoutePoints { get; set; }

		public int TripId { get; set; }
	}
}
