using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using TripsServiceBLL.DTO.Trips;
using TripsServiceBLL.Infrastructure.Exceptions;
using TripsServiceBLL.Interfaces;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Interfaces;

namespace TripsServiceBLL.Commands.Trips;

public class EditTripCommandAsync : ICommandAsync<EditTripDTO>
{
	private readonly IImageService _imageService;
	private readonly IRoutePointService _routePointService;
	private readonly ITripService _tripService;

	private readonly IMapper _mapper;

	private readonly IUnitOfWork _unitOfWork;

	public EditTripCommandAsync(
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

	public async Task ExecuteAsync(EditTripDTO dto)
	{
		Trip trip = await _unitOfWork.Trips.GetByIdAsync(dto.Id);
		if (dto is AdminEditTripDTO adminDto)
			_mapper.Map(adminDto, trip);
		else
			_mapper.Map(dto, trip);

		using IDbContextTransaction transaction = _unitOfWork.BeginTransaction();
		try
		{
			await _tripService.UpdateAsync(trip);
			await _routePointService.DeleteByTripIdAsync(trip.Id);
			await _routePointService.AddTripRoutePointsAsync(trip.Id, dto.RoutePointsAsString);
			await _imageService.SaveTripImagesAsync(trip.Id, trip.UserId, dto.ImagesAsFiles);
			await transaction.CommitAsync();
		}
		catch (Exception)
		{
			await transaction.RollbackAsync();
			throw new DbOperationException();
		}
	}
}
