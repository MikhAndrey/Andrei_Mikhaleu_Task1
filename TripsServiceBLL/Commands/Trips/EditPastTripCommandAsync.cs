using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using TripsServiceBLL.DTO.Trips;
using TripsServiceBLL.Infrastructure.Exceptions;
using TripsServiceBLL.Interfaces;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Interfaces;

namespace TripsServiceBLL.Commands.Trips;

public class EditPastTripCommandAsync : ICommandAsync<EditPastTripDTO>
{
	private readonly IMapper _mapper;

	private readonly IImageService _imageService;
	private readonly ITripService _tripService;

	private readonly IUnitOfWork _unitOfWork;

	public EditPastTripCommandAsync(
		IImageService imageService,
		ITripService tripService,
		IMapper mapper,
		IUnitOfWork unitOfWork
	)
	{
		_imageService = imageService;
		_tripService = tripService;
		_mapper = mapper;
		_unitOfWork = unitOfWork;
	}

	public async Task ExecuteAsync(EditPastTripDTO dto)
	{
		Trip trip = await _unitOfWork.Trips.GetByIdAsync(dto.Id);

		_mapper.Map(dto, trip);

		using IDbContextTransaction transaction = _unitOfWork.BeginTransaction();
		try
		{
			await _tripService.UpdateAsync(trip);
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
