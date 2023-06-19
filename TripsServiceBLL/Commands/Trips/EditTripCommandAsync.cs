using AutoMapper;
using Microsoft.AspNetCore.Http;
using TripsServiceBLL.DTO.Trips;
using TripsServiceBLL.Infrastructure;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Utils;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Commands.Trips
{
    public class EditTripCommandAsync : ICommandAsync
    {
        private readonly EditTripDTO _trip;

        private readonly IRoutePointService _routePointService;

        private readonly IImageService _imageService;

        private readonly ITripService _tripService;

        private readonly int _id;

        private readonly List<IFormFile> _images;

        private readonly string _webRootPath;

        private readonly string _routePoints;

        private readonly IMapper _mapper;

        public EditTripCommandAsync(
            EditTripDTO trip,
            int id,
            List<IFormFile> images,
            IRoutePointService routePointService,
            IImageService imageService,
            ITripService tripService,
            string webRootPath,
            string routePoints,
            IMapper mapper
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
            _mapper = mapper;
        }

        public async Task ExecuteAsync()
        {
            Trip? trip = await _tripService.GetByIdAsync(_id);
            if (trip == null)
            {
                throw new EntityNotFoundException(Constants.TripNotExistsMessage);
            }

            _ = _mapper.Map(_trip, trip);
            await _imageService.UploadImagesAsync(trip, _images, _webRootPath);
            trip.RoutePoints.Clear();
            _routePointService.ParseAndAddRoutePoints(trip, _routePoints);
            await _tripService.UpdateAsync(trip);
        }
    }
}
