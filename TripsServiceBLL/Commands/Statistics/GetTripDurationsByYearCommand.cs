using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Utils;

namespace TripsServiceBLL.Commands.Statistics
{
	public class GetTripDurationsByYearCommand : IAsyncCommand<List<DurationInMonth>>
	{
		private readonly ITripService _tripService;

		private readonly int _userId;

		private readonly int _year;

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
