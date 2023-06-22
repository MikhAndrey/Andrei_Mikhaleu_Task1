using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using TripsServiceBLL.Helpers;
using TripsServiceBLL.DTO.Trips;
using TripsServiceBLL.Infrastructure.Exceptions;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Utils;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Commands.Trips
{
    public class CreateTripCommandAsync : ICommandAsync<CreateTripDTO>
    {
        private readonly IImageService _imageService;

        private readonly ITripService _tripService;

        private readonly IUserService _userService;

        private readonly IMapper _mapper;

        private readonly IWebHostEnvironment _env;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateTripCommandAsync(
            IImageService imageService,  
            ITripService tripService,  
            IUserService userService,  
            IMapper mapper,
            IWebHostEnvironment env,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _imageService = imageService;
            _tripService = tripService;
            _userService = userService;
            _mapper = mapper;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task ExecuteAsync(CreateTripDTO dto)
        {
			int userId = UserHelper.GetUserIdFromClaims(_httpContextAccessor.HttpContext.User.Claims);
			bool userExists = _userService.Exists(userId);
            if (!userExists)
            {
                throw new EntityNotFoundException(Constants.GetEntityNotFoundMessage("user"));
            }

            Trip trip = _mapper.Map<Trip>(dto);
            trip.UserId = userId;
            await _tripService.AddAsync(trip);
            await _imageService.UploadImagesAsync(trip, dto.ImagesAsFiles, _env.WebRootPath);
            await _tripService.UpdateAsync(trip);
        }
    }
}
