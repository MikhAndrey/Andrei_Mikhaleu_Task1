using TripsServiceBLL.Interfaces;
using TripsServiceBLL.DTO.RoutePoints;

namespace TripsServiceBLL.Commands.RoutePoints
{
	public class GetRoutePointsCoordinatesCommand : ICommand<IQueryable<RoutePointCoordinatesDTO>>
	{
		private IRoutePointService _routePointService;

		private int _userId;

		private int _year;

		public GetRoutePointsCoordinatesCommand(IRoutePointService routePointService, int userId, int year)
		{
			_routePointService = routePointService;
			_userId = userId;
			_year = year;
		}

		public IQueryable<RoutePointCoordinatesDTO> Execute()
		{
			return _routePointService.GetRoutePointsByYear(_year, _userId);
		}
	}
}
