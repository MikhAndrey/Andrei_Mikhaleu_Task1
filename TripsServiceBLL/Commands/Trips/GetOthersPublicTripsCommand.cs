using TripsServiceBLL.DTO.Trips;
using TripsServiceBLL.Interfaces;

namespace TripsServiceBLL.Commands.Trips
{
	public class GetOthersPublicTripsCommand : ICommand<IQueryable<ReadTripDTOExtended>>
	{
		private readonly ITripService _tripService;

		private readonly int _userId;

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
