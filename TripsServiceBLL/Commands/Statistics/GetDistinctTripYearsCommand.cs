using TripsServiceBLL.DTO.Statistics;
using TripsServiceBLL.Interfaces;

namespace TripsServiceBLL.Commands.Statistics
{
    public class GetDistinctTripYearsCommand : IAsyncCommand<YearsStatisticsDTO>
    {
        private readonly ITripService _tripService;

        private readonly int _userId;

        public GetDistinctTripYearsCommand(ITripService tripService, int userId)
        {
            _tripService = tripService;
            _userId = userId;
        }

        public async Task<YearsStatisticsDTO> ExecuteAsync()
        {
            return _tripService.GetYearsOfUserTrips(_userId);
        }
    }
}
