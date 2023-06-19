using AutoMapper;
using Microsoft.AspNetCore.Http;
using TripsServiceBLL.DTO.Trips;
using TripsServiceBLL.Infrastructure.Exceptions;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Utils;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Commands.Trips
{
    public class CreateTripCommandAsync : ICommandAsync
    {
        private readonly CreateTripDTO _trip;

        private readonly IRoutePointService _routePointService;

        private readonly IImageService _imageService;

        private readonly ITripService _tripService;

        private readonly IUserService _userService;

        private readonly List<IFormFile> _images;

        private readonly string _webRootPath;

        private readonly string _routePoints;

        private readonly int _userId;

        private readonly IMapper _mapper;

        public CreateTripCommandAsync(
            CreateTripDTO trip,
            List<IFormFile> images,
            IRoutePointService routePointService,
            IImageService imageService,
            ITripService tripService,
            IUserService userService,
            IMapper mapper,
            string webRootPath,
            string routePoints,
            int userId
        )
        {
            _trip = trip;
            _images = images;
            _routePointService = routePointService;
            _imageService = imageService;
            _tripService = tripService;
            _userService = userService;
            _mapper = mapper;
            _webRootPath = webRootPath;
            _routePoints = routePoints;
            _userId = userId;
        }

        public async Task ExecuteAsync()
        {
            bool userExists = _userService.Exists(_userId);
            if (!userExists)
            {
                throw new EntityNotFoundException(Constants.UserNotFoundMessage);
            }

            Trip trip = _mapper.Map<Trip>(_trip);
            _routePointService.ParseAndAddRoutePoints(trip, _routePoints);
            trip.UserId = _userId;
            await _tripService.AddAsync(trip);
            await _imageService.UploadImagesAsync(trip, _images, _webRootPath);
            await _tripService.UpdateAsync(trip);
        }
    }
}
