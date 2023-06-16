using AutoMapper;
using TripsServiceBLL.DTO.Drivers;
using TripsServiceBLL.DTO.Feedbacks;
using TripsServiceBLL.Infrastructure;
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
            return drivers.Select(d => _mapper.Map<Driver, ReadDriverDTO>(d, opt =>
                opt.AfterMap((src, dest) => dest.AverageRating = ComputeAverageRating(src))))
                .OrderByDescending(d => d.AverageRating);
        }

        public async Task<DriverDetailsDTO> GetDriverDetailsAsync(int driverId)
        {
            Driver? driver = await _unitOfWork.Drivers.GetByIdAsync(driverId);
            return driver == null
                ? throw new EntityNotFoundException(Constants.DriverNotExistsMessage)
                : _mapper.Map<Driver, DriverDetailsDTO>(driver, opt =>
                opt.AfterMap((src, dest) =>
                {
                    dest.AverageRating = ComputeAverageRating(src);
                    dest.Feedbacks = src.Trips.Where(t => t.Feedback != null)
                    .Select(t => _mapper.Map<ReadFeedbackDTO>(t));
                })
            );
        }

        private double ComputeAverageRating(Driver driver)
        {
            return Math.Round(driver.Trips.Where(t => t.Feedback != null)
                .Average(t => (double?)t.Feedback.Rating) ?? 0, 1);
        }
    }
}
