using TripsServiceBLL.Interfaces;
using TripsServiceBLL.DTO.Trips;

namespace TripsServiceBLL.Commands.Trips
{
	public class GetTripsHistoryCommand : ICommand<IQueryable<ReadTripDTO>>
	{
		private ITripService _tripService;

		private int _userId;

		public GetTripsHistoryCommand(ITripService tripService, int userId)
		{
			_tripService = tripService;
			_userId = userId;
		}

		public IQueryable<ReadTripDTO> Execute()
		{
			return _tripService.GetHistoryOfTripsByUserId(_userId);
		}
	}
}
