using Microsoft.AspNetCore.Http;
using TripsServiceDAL.Entities;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.DTO.Trips;

namespace TripsServiceBLL.Commands.Trips
{
    public class EditTripCommand : IAsyncCommand
    {
        private readonly ExistingTripDTO _trip;

        private readonly IRoutePointService _routePointService;

        private readonly IImageService _imageService;

        private readonly ITripService _tripService;

        private readonly int _id;

        private readonly List<IFormFile> _images;

        private readonly string _webRootPath;

        private readonly string _routePoints;

        public EditTripCommand(
            ExistingTripDTO trip,
            int id,
            List<IFormFile> images,
            IRoutePointService routePointService,
            IImageService imageService,
            ITripService tripService,
            string webRootPath,
            string routePoints
        )
        {
            _trip = trip;
            _id = id;
            _images = images;
            _routePointService = routePointService;
            _imageService = imageService;
            _tripService = tripService;
            _webRootPath = webRootPath;
            _routePoints = routePoints;
        }

        public async Task ExecuteAsync()
        {
            Trip? trip = await _tripService.GetByIdAsync(_id);
            if (trip != null)
            {
                trip.Name = _trip.Name;
                trip.Description = _trip.Description;
                trip.Distance = _trip.Distance;
                trip.Public = _trip.Public;
                trip.StartTime = _trip.StartTime;
                trip.EndTime = _trip.EndTime;
                trip.StartTimeZoneOffset = _trip.StartTimeZoneOffset;
                trip.FinishTimeZoneOffset = _trip.FinishTimeZoneOffset;
                await _imageService.UploadImagesAsync(trip, _images, _webRootPath);
                trip.RoutePoints.Clear();
                _routePointService.ParseAndAddRoutePoints(trip, _routePoints);
                await _tripService.UpdateAsync(trip);
            }
        }
    }
}
