using TripsServiceBLL.Interfaces;
using TripsServiceBLL.DTO.Statistics;

namespace TripsServiceBLL.Commands.Statistics
{
	public class GetDistinctTripYearsCommand : IAsyncCommand<YearsStatisticsDTO>
	{
		private ITripService _tripService;

		private int _userId;

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
