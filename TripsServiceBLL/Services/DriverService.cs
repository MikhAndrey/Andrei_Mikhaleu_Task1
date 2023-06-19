using AutoMapper;
using TripsServiceBLL.DTO.Drivers;
using TripsServiceBLL.Infrastructure.Exceptions;
using TripsServiceBLL.Interfaces;
using TripsServiceBLL.Utils;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Interfaces;

namespace TripsServiceBLL.Services
{
    public class DriverService : IDriverService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public DriverService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<ReadDriverDTO> GetDriversOverall()
        {
            IEnumerable<Driver> drivers = _unitOfWork.Drivers.GetAll().AsEnumerable();
            return drivers.Select(_mapper.Map<Driver, ReadDriverDTO>)
                .OrderByDescending(el => el.AverageRating);
        }

        public async Task<DriverDetailsDTO> GetDriverDetailsAsync(int driverId)
        {
            Driver? driver = await _unitOfWork.Drivers.GetByIdAsync(driverId);
            return driver == null
                ? throw new EntityNotFoundException(Constants.GetEntityNotExistsMessage("driver"))
                : _mapper.Map<Driver, DriverDetailsDTO>(driver);
        }
    }
}
