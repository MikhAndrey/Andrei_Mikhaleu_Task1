using TripsServiceDAL.Entities;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.DTO.RoutePoints;

namespace TripsServiceBLL.Commands.RoutePoints
{
    public class GetRoutePointsCoordinatesCommand : IAsyncCommand<IQueryable<RoutePointCoordinatesDTO>>
    {
        private IRoutePointService _routePointService;

        private IUserService _userService;

        private string _userName;

        private int _year;

        public GetRoutePointsCoordinatesCommand(IRoutePointService routePointService, IUserService userService, string userName, int year)
        {
            _routePointService = routePointService;
            _userService = userService;
            _userName = userName;
            _year = year;
        }

        public async Task<IQueryable<RoutePointCoordinatesDTO>> ExecuteAsync()
        {
            User? user = await _userService.GetByUserNameAsync(_userName);
            return _routePointService.GetRoutePointsByYear(_year, user.UserId);
        }
    }
}
