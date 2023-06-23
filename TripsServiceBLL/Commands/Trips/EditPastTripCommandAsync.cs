using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using TripsServiceBLL.DTO.Trips;
using TripsServiceBLL.Infrastructure.Exceptions;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Utils;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Commands.Trips
{
    public class EditPastTripCommandAsync : ICommandAsync<EditPastTripDTO>
    {
        private readonly IImageService _imageService;

        private readonly ITripService _tripService;

        private readonly IWebHostEnvironment _env;

        private readonly IMapper _mapper;

        public EditPastTripCommandAsync(
            IImageService imageService,
            ITripService tripService,
            IWebHostEnvironment env,
            IMapper mapper
        )
        {
            _imageService = imageService;
            _tripService = tripService;
            _env = env;
            _mapper = mapper;
        }

        public async Task ExecuteAsync(EditPastTripDTO dto)
        {
            Trip? trip = await _tripService.GetByIdWithImagesAsync(dto.Id);
            if (trip == null)
            {
                throw new EntityNotFoundException(Constants.GetEntityNotExistsMessage("trip"));
            }

            _mapper.Map(dto, trip);
            await _imageService.UploadImagesAsync(trip.Id, trip.UserId, dto.ImagesAsFiles, _env.WebRootPath);
            await _tripService.UpdateAsync(trip);
        }
    }
}
