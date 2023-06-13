using Microsoft.AspNetCore.Http;
using TripsServiceBLL.DTO.Trips;
using TripsServiceBLL.Infrastructure;
using TripsServiceBLL.Interfaces;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Commands.Trips
{
	public class CreateTripCommand : IAsyncCommand
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

		public CreateTripCommand(
			CreateTripDTO trip,
			List<IFormFile> images,
			IRoutePointService routePointService,
			IImageService imageService,
			ITripService tripService,
			IUserService userService,
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
			_webRootPath = webRootPath;
			_routePoints = routePoints;
			_userId = userId;
		}

		public async Task ExecuteAsync()
		{
			User? user = await _userService.GetByIdAsync(_userId);
			if (user != null)
			{
				_tripService.FixTimeOfNewTripForTimeZones(_trip);
				Trip trip = CustomMapper<CreateTripDTO, Trip>.Map(_trip);
				_routePointService.ParseAndAddRoutePoints(trip, _routePoints);
				trip.User = user;
				await _tripService.AddAsync(trip);
				await _imageService.UploadImagesAsync(trip, _images, _webRootPath);
				await _tripService.UpdateAsync(trip);
			}
		}
	}
}
