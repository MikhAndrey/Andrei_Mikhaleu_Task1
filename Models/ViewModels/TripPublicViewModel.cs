using Andrei_Mikhaleu_Task1.Models.Entities.Common;

namespace Andrei_Mikhaleu_Task1.Models.ViewModels
{
    public class TripPublicViewModel : TripViewModel
    {
        public string UserName { get; set; }

        public TripPublicViewModel(Trip trip) : base(trip)
        {
            UserName = trip.User.UserName;
        }
    }
}
