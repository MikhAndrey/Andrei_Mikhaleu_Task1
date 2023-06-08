﻿using Microsoft.AspNetCore.Http;
using TripsServiceBLL.Services;
using TripsServiceDAL.Entities;
using TripsServiceBLL.Infrastructure;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.DTO.Trips;

namespace TripsServiceBLL.Commands.Trips
{
    public class CreateTripCommand : IAsyncCommand
    {
        private readonly NewTripDTO _trip;

        private readonly RoutePointService _routePointService;

        private readonly ImageService _imageService;

        private readonly TripService _tripService;

        private readonly UserService _userService;

        private readonly List<IFormFile> _images;

        private readonly string _webRootPath;

        private readonly string _routePoints;

        private readonly string? _userName;

        public CreateTripCommand(
            NewTripDTO trip,
            List<IFormFile> images,
            RoutePointService routePointService,
            ImageService imageService,
            TripService tripService,
            UserService userService,
            string webRootPath,
            string routePoints,
            string? userName
        )
        {
            _trip = trip;
            _images = images;
            _routePointService = routePointService;
            _imageService = imageService;
            _tripService = tripService;
            _userService = userService;
            _webRootPath = webRootPath;
            _routePoints = routePoints;
            _userName = userName;
        }

        public async Task ExecuteAsync()
        {
            User? user = await _userService.GetByUserNameAsync(_userName);
            if (user == null)
                throw new ValidationException("User was not found", "");
            Trip trip = CustomMapper<NewTripDTO, Trip>.Map(_trip);
            await _imageService.UploadImages(trip, _images, _webRootPath);
            _routePointService.ParseAndAddRoutePoints(trip, _routePoints);
            trip.User = user;
            await _tripService.AddAsync(trip);
        }
    }
}