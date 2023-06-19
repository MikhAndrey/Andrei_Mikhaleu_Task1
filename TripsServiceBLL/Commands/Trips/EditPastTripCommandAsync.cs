using AutoMapper;
using Microsoft.AspNetCore.Http;
using TripsServiceBLL.DTO.Trips;
using TripsServiceBLL.Infrastructure;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Utils;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Commands.Trips
{
    public class EditPastTripCommandAsync : ICommandAsync
    {
        private readonly EditPastTripDTO _trip;

        private readonly IImageService _imageService;

        private readonly ITripService _tripService;

        private readonly int _id;

        private readonly List<IFormFile> _images;

        private readonly string _webRootPath;

        private readonly IMapper _mapper;

        public EditPastTripCommandAsync(
            EditPastTripDTO trip,
            int id,
            List<IFormFile> images,
            IImageService imageService,
            ITripService tripService,
            string webRootPath,
            IMapper mapper
        )
        {
            _trip = trip;
            _id = id;
            _images = images;
            _imageService = imageService;
            _tripService = tripService;
            _webRootPath = webRootPath;
            _mapper = mapper;
        }

        public async Task ExecuteAsync()
        {
            Trip? trip = await _tripService.GetByIdWithImagesAsync(_id);
            if (trip == null)
            {
                throw new EntityNotFoundException(Constants.TripNotExistsMessage);
            }

            _ = _mapper.Map(_trip, trip);
            await _imageService.UploadImagesAsync(trip, _images, _webRootPath);
            await _tripService.UpdateAsync(trip);
        }
    }
}
