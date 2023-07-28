using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using TripsServiceBLL.DTO.Trips;
using TripsServiceBLL.Infrastructure.Exceptions;
using TripsServiceBLL.Interfaces;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Interfaces;

namespace TripsServiceBLL.Commands.Trips;

public class CreateTripCommandAsync : ICommandAsync<CreateTripDTO>
{
	private readonly IImageService _imageService;
	private readonly IRoutePointService _routePointService;
	private readonly ITripService _tripService;

	private readonly IMapper _mapper;

	private readonly IUnitOfWork _unitOfWork;

	public CreateTripCommandAsync(
		IImageService imageService,
		ITripService tripService,
		IRoutePointService routePointService,
		IMapper mapper,
		IUnitOfWork unitOfWork
	)
	{
		_imageService = imageService;
		_tripService = tripService;
		_routePointService = routePointService;
		_mapper = mapper;
		_unitOfWork = unitOfWork;
	}

	public async Task ExecuteAsync(CreateTripDTO dto)
	{
		Trip trip = dto is AdminCreateTripDTO adminCreateTripDto ? _mapper.Map<Trip>(adminCreateTripDto) : _mapper.Map<Trip>(dto);

		using IDbContextTransaction transaction = _unitOfWork.BeginTransaction();
		try
		{
			await _tripService.AddAsync(trip);
			await _routePointService.AddTripRoutePointsAsync(trip.Id, dto.RoutePointsAsString);
			await _imageService.SaveTripImagesAsync(trip.Id, dto.ImagesAsFiles);
			await transaction.CommitAsync();
		}
		catch (Exception)
		{
			await transaction.RollbackAsync();
			throw new DbOperationException();
		}
	}
}
