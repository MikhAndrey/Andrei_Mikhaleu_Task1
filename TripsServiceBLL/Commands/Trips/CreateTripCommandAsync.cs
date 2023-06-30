using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using TripsServiceBLL.DTO.Trips;
using TripsServiceBLL.Infrastructure.Exceptions;
using TripsServiceBLL.Interfaces;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Interfaces;

namespace TripsServiceBLL.Commands.Trips
{
	public class CreateTripCommandAsync : ICommandAsync<CreateTripDTO>
	{
		private readonly IImageService _imageService;
		private readonly ITripService _tripService;
		private readonly IUserService _userService;
		private readonly IRoutePointService _routePointService;

		private readonly IMapper _mapper;

		private readonly IWebHostEnvironment _env;

		private readonly IHttpContextAccessor _httpContextAccessor;

		private readonly IUnitOfWork _unitOfWork;

		public CreateTripCommandAsync(
			IImageService imageService,
			ITripService tripService,
			IUserService userService,
			IRoutePointService routePointService,
			IMapper mapper,
			IWebHostEnvironment env,
			IHttpContextAccessor httpContextAccessor,
			IUnitOfWork unitOfWork
		)
		{
			_imageService = imageService;
			_tripService = tripService;
			_userService = userService;
			_routePointService = routePointService;
			_mapper = mapper;
			_env = env;
			_httpContextAccessor = httpContextAccessor;
			_unitOfWork = unitOfWork;
		}

		public async Task ExecuteAsync(CreateTripDTO dto)
		{
			int userId = _userService.GetCurrentUserId();

			Trip trip = _mapper.Map<Trip>(dto);
			trip.UserId = userId;

			List<RoutePoint>? routePoints = _routePointService.ParseRoutePointsFromString(dto.RoutePointsAsString);

			List<string> fileNames = _imageService.GenerateImagesFileNames(dto.ImagesAsFiles);

			using (IDbContextTransaction transaction = _unitOfWork.BeginTransaction())
			{
				try
				{
					await _tripService.AddAsync(trip);
					await _routePointService.AddTripRoutePointsAsync(trip.Id, routePoints);
					await _imageService.AddTripImagesAsync(fileNames, trip.Id);
					await transaction.CommitAsync();
				}
				catch (Exception)
				{
					await transaction.RollbackAsync();
					throw new DbOperationException();
				}
			}

			await _imageService.SaveTripImagesFilesAsync(trip.Id, userId, fileNames, dto.ImagesAsFiles, _env.WebRootPath);
		}
	}
}
