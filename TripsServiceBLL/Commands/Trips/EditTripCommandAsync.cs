using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using TripsServiceBLL.DTO.Trips;
using TripsServiceBLL.Infrastructure.Exceptions;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Utils;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Interfaces;

namespace TripsServiceBLL.Commands.Trips
{
    public class EditTripCommandAsync : ICommandAsync<EditTripDTO>
    {
        private readonly IImageService _imageService;

        private readonly ITripService _tripService;

        private readonly IRoutePointService _routePointService;

        private readonly IWebHostEnvironment _env;

        private readonly IMapper _mapper;

        private readonly IUnitOfWork _unitOfWork;

        public EditTripCommandAsync(
            IImageService imageService,
            ITripService tripService,
            IRoutePointService routePointService,
            IWebHostEnvironment env,
            IMapper mapper,
            IUnitOfWork unitOfWork
        )
        {
            _imageService = imageService;
            _tripService = tripService;
            _routePointService = routePointService;
            _env = env;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(EditTripDTO dto)
        {
            Trip? trip = await _tripService.GetByIdAsync(dto.Id);
            if (trip == null)
            {
                throw new EntityNotFoundException(Constants.GetEntityNotExistsMessage("trip"));
            }

            _mapper.Map(dto, trip);

            List<RoutePoint>? routePoints = _routePointService.ParseRoutePointsFromString(dto.RoutePointsAsString);

            List<string> fileNames = _imageService.GenerateImagesFileNames(dto.ImagesAsFiles);

            using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    await _tripService.UpdateAsync(trip);
                    await _routePointService.DeleteByTripIdAsync(trip.Id);
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

            await _imageService.SaveTripImagesFilesAsync(trip.Id, trip.UserId, fileNames, dto.ImagesAsFiles, _env.WebRootPath);
        }
    }
}
