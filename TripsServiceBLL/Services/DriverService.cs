using TripsServiceBLL.DTO.Drivers;
using TripsServiceBLL.Interfaces;
using TripsServiceDAL.Interfaces;

namespace TripsServiceBLL.Services
{
    public class DriverService : IDriverService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DriverService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<ReadDriverDTO> GetDriversOverall()
        {
            return _unitOfWork.Drivers.GetAll().Select(d => new ReadDriverDTO
            {
                Id = d.Id,
                Name = d.Name,
                Experience = d.Experience,
                AverageRating = d.Trips.Average(t => (double?)t.Feedback.Rating ?? 0),
                FirstPhoto = d.Photos.FirstOrDefault()
            }).OrderByDescending(d => d.AverageRating);
        }
    }
}
