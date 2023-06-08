using System.Text.Json;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Infrastructure;
using TripsServiceBLL.DTO.RoutePoints;

namespace TripsServiceBLL.Services
{
    public class RoutePointService
    {
        private readonly UnitOfWork _unitOfWork;

        public RoutePointService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void ParseAndAddRoutePoints(Trip trip, string routePoints)
        {
            List<RoutePoint>? parsedRoutePoints = JsonSerializer.Deserialize<List<RoutePoint>>(routePoints);

            if (parsedRoutePoints != null)
            {
                foreach (RoutePoint routePoint in parsedRoutePoints)
                {
                    trip.RoutePoints.Add(routePoint);
                }
            }
        }

        public IQueryable<RoutePointCoordinatesDTO> GetRoutePointsByYear(int year, int userId)
        {
            IQueryable<RoutePointCoordinatesDTO> dataPoints = _unitOfWork.RoutePoints
                .GetRoutePointsByYear(year, userId)
                .Select(rp => new RoutePointCoordinatesDTO() { Latitude = rp.Latitude, Longitude = rp.Longitude });
            return dataPoints;
        }
    }
}
