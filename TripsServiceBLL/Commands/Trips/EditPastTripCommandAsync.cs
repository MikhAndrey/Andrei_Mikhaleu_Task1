using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Storage;
using TripsServiceBLL.DTO.Trips;
using TripsServiceBLL.Infrastructure.Exceptions;
using TripsServiceBLL.Interfaces;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Interfaces;

namespace TripsServiceBLL.Commands.Trips
{
    public class EditPastTripCommandAsync : ICommandAsync<EditPastTripDTO>
    {
        private readonly IImageService _imageService;
        private readonly ITripService _tripService;

        private readonly IWebHostEnvironment _env;

        private readonly IMapper _mapper;

        private readonly IUnitOfWork _unitOfWork;

        public EditPastTripCommandAsync(
            IImageService imageService,
            ITripService tripService,
            IWebHostEnvironment env,
            IMapper mapper,
            IUnitOfWork unitOfWork
        )
        {
            _imageService = imageService;
            _tripService = tripService;
            _env = env;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(EditPastTripDTO dto)
        {
            Trip trip = await _unitOfWork.Trips.GetByIdAsync(dto.Id);

            _mapper.Map(dto, trip);

            List<string> fileNames = _imageService.GenerateImagesFileNames(dto.ImagesAsFiles);

            using (IDbContextTransaction transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    await _tripService.UpdateAsync(trip);
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
