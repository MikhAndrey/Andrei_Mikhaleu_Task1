using TripsServiceBLL.DTO.RoutePoints;
using TripsServiceBLL.Interfaces;

namespace TripsServiceBLL.Commands.RoutePoints
{
    public class GetRoutePointsCoordinatesCommand : ICommand<IQueryable<RoutePointCoordinatesDTO>>
    {
        private readonly IRoutePointService _routePointService;

        private readonly int _userId;

        private readonly int _year;

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
