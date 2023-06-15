using TripsServiceBLL.DTO.Trips;
using TripsServiceBLL.Interfaces;

namespace TripsServiceBLL.Commands.Trips
{
	public class GetUserTripsCommand : ICommand<IQueryable<ReadTripDTO>>
	{
		private readonly ITripService _tripService;

		private readonly int _userId;

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
