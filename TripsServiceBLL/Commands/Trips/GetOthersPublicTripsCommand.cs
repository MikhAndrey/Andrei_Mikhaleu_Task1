using TripsServiceBLL.Interfaces;
using TripsServiceBLL.DTO.Trips;

namespace TripsServiceBLL.Commands.Trips
{
    public class GetOthersPublicTripsCommand : ICommand<IQueryable<ReadTripDTOExtended>>
    {
        private ITripService _tripService;

        private int _userId;

        public GetOthersPublicTripsCommand(ITripService tripService, int userId)
        {
            _tripService = tripService;
            _userId = userId;
        }

        public IQueryable<ReadTripDTOExtended> Execute()
        {
            return _tripService.GetOthersPublicTrips(_userId);
        }
    }
}
