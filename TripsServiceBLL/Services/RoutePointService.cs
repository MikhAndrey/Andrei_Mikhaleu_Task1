using System.Text.Json;
using TripsServiceBLL.DTO.RoutePoints;
using TripsServiceBLL.Interfaces;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Interfaces;

namespace TripsServiceBLL.Services
{
    public class RoutePointService : IRoutePointService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoutePointService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void DeleteByTripId(int tripId)
        {
            IQueryable<RoutePoint> routePointsToDelete = _unitOfWork.RoutePoints.GetRoutePointsByTripId(tripId);
            foreach (RoutePoint routePoint in routePointsToDelete)
            {
                _unitOfWork.RoutePoints.Delete(routePoint);
            }
        }

        public async Task ParseAndAddRoutePoints(int tripId, string routePoints)
        {
            List<RoutePoint>? parsedRoutePoints = JsonSerializer.Deserialize<List<RoutePoint>>(routePoints);

            if (parsedRoutePoints != null)
            {
                foreach (RoutePoint routePoint in parsedRoutePoints)
                {
                    routePoint.TripId = tripId;
                    await _unitOfWork.RoutePoints.AddAsync(routePoint);
                }
                await _unitOfWork.SaveAsync();
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
