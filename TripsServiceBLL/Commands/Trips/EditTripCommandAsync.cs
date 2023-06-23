using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using TripsServiceBLL.DTO.Trips;
using TripsServiceBLL.Infrastructure.Exceptions;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Utils;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Commands.Trips
{
    public class EditTripCommandAsync : ICommandAsync<EditTripDTO>
    {
        private readonly IImageService _imageService;

        private readonly ITripService _tripService;

        private readonly IRoutePointService _routePointService;

        private readonly IWebHostEnvironment _env;

        private readonly IMapper _mapper;

        public EditTripCommandAsync(
            IImageService imageService,
            ITripService tripService,
            IRoutePointService routePointService,
            IWebHostEnvironment env,
            IMapper mapper
        )
        {
            _imageService = imageService;
            _tripService = tripService;
            _routePointService = routePointService;
            _env = env;
            _mapper = mapper;
        }

        public async Task ExecuteAsync(EditTripDTO dto)
        {
            Trip? trip = await _tripService.GetByIdAsync(dto.Id);
            if (trip == null)
            {
                throw new EntityNotFoundException(Constants.GetEntityNotExistsMessage("trip"));
            }

            _mapper.Map(dto, trip);
            _routePointService.DeleteByTripId(trip.Id);
            await _routePointService.ParseAndAddRoutePoints(trip.Id, dto.RoutePointsAsString);
            await _imageService.UploadImagesAsync(trip.Id, trip.UserId, dto.ImagesAsFiles, _env.WebRootPath);
            await _tripService.UpdateAsync(trip);
        }
    }
}
