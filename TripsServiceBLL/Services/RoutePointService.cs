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

        public async Task DeleteByTripIdAsync(int tripId)
        {
            IQueryable<RoutePoint> routePointsToDelete = _unitOfWork.RoutePoints.GetRoutePointsByTripId(tripId);
            foreach (RoutePoint routePoint in routePointsToDelete)
            {
                _unitOfWork.RoutePoints.Delete(routePoint);
            }
            await _unitOfWork.SaveAsync();
        }

        public List<RoutePoint>? ParseRoutePointsFromString(string routePoints)
        {
            return JsonSerializer.Deserialize<List<RoutePoint>>(routePoints);
        }

        public async Task AddTripRoutePointsAsync(int tripId, List<RoutePoint>? routePoints)
        {
            if (routePoints != null)
            {
                foreach (RoutePoint routePoint in routePoints)
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
