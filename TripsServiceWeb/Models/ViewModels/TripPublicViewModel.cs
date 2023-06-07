using TripsServiceBLL.DTO;

namespace Andrei_Mikhaleu_Task1.Models.ViewModels
{
    public class TripPublicViewModel : TripViewModel
    {
        public string UserName { get; set; }

        public TripPublicViewModel(TripDTO trip) : base(trip)
        {
            UserName = trip.User.UserName;
        }
    }
}
