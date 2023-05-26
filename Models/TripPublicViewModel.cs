using Andrei_Mikhaleu_Task1.Models.Entities;

namespace Andrei_Mikhaleu_Task1.Models
{
    public class TripPublicViewModel : TripViewModel
    {
        public string UserName { get; set; }

        public TripPublicViewModel(Trip trip) : base(trip) 
        {
            this.UserName = trip.User.UserName;
        }
	}
}
