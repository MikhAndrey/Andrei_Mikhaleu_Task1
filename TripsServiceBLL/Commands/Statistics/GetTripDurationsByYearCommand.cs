using TripsServiceBLL.Utils;
using TripsServiceBLL.Interfaces;

namespace TripsServiceBLL.Commands.Statistics
{
    public class GetTripDurationsByYearCommand : IAsyncCommand<List<DurationInMonth>>
    {
        private ITripService _tripService;

        private int _userId;

        private int _year;

        public GetTripDurationsByYearCommand(ITripService tripService, int userId, int year)
        {
            _tripService = tripService;
            _userId = userId;
            _year = year;
        }

        public async Task<List<DurationInMonth>> ExecuteAsync()
        {
            return await _tripService.GetTotalDurationByMonthsAsync(_year, _userId);
        }
    }
}
