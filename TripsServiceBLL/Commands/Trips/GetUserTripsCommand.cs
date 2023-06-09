using TripsServiceBLL.Interfaces;
using TripsServiceBLL.DTO.Trips;

namespace TripsServiceBLL.Commands.Trips
{
    public class GetUserTripsCommand : ICommand<IQueryable<ReadTripDTO>>
    {
        private ITripService _tripService;

        private int _userId;

        public GetUserTripsCommand(ITripService tripService, int userId)
        {
            _tripService = tripService;
            _userId = userId;
        }

        public IQueryable<ReadTripDTO> Execute()
        {
            return _tripService.GetTripsByUserId(_userId);
        }
    }
}
