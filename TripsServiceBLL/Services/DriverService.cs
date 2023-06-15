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
                AverageRating = Math.Round(d.Trips.Where(t => t.Feedback != null)
                .Average(t => (double?)t.Feedback.Rating) ?? 0, 1),
                FirstPhoto = d.Photos.FirstOrDefault()
            }).OrderByDescending(d => d.AverageRating);
        }
    }
}
