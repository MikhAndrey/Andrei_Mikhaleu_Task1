using TripsServiceBLL.DTO;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Infrastructure;
using TripsServiceBLL.Infrastructure;
using Microsoft.EntityFrameworkCore;
using TripsServiceBLL.Utils;

namespace TripsServiceBLL.Services
{
    public class TripService
    {
        private readonly UnitOfWork _unitOfWork;

        public TripService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Trip?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Trips.GetByIdAsync(id);
        }

        public void SetNewTimeForStartingTrip(Trip trip)
        {
            trip.EndTime -= trip.StartTime - DateTime.UtcNow;
            trip.EndTime = DateTime.Parse(trip.EndTime.ToString("dd.MM.yyyy HH:mm"));
            trip.StartTime = DateTime.UtcNow;
            trip.StartTime = DateTime.Parse(trip.StartTime.ToString("dd.MM.yyyy HH:mm"));
        }

        public void SetNewTimeForEndingTrip(Trip trip)
        {
            trip.EndTime = DateTime.UtcNow;
            trip.EndTime = DateTime.Parse(trip.EndTime.ToString("dd.MM.yyyy HH:mm"));
        }

        public async Task UpdateAsync(Trip trip)
        {
            _unitOfWork.Trips.Update(trip);
            await _unitOfWork.Save();
        }

        public async Task DeleteAsync(Trip trip)
        {
            _unitOfWork.Trips.Delete(trip);
            await _unitOfWork.Save();
        }

        public async Task AddAsync(Trip trip)
        {
            await _unitOfWork.Trips.AddAsync(trip);
            await _unitOfWork.Save();
        }

        public async Task<TripDTO> InitializeTripDTOAsync(int tripId, int userId)
        {
            Trip? trip = await GetByIdAsync(tripId);
            if (trip == null)
            {
                throw new ValidationException("Trip was not found", "");
            }
            else
            {
                TripDTO newTrip = CustomMapper<Trip, TripDTO>.Map(trip);
                newTrip.IsCurrentUserTrip = trip.User.UserId == userId;
                return newTrip;
            }
        }

        public async Task<ExtendedExistingTripDTO> InitializeExtendedExistingTripAsync(int tripId)
        {
            Trip? trip = await GetByIdAsync(tripId);
            if (trip == null)
            {
                throw new ValidationException("Trip was not found", "");
            }
            else
            {
                return CustomMapper<Trip, ExtendedExistingTripDTO>.Map(trip);
            }
        }

        public List<TripDTO> GetOthersPublicTrips(int userId)
        {
            IQueryable<Trip> rawTrips = _unitOfWork.Trips.GetOthersPublicTrips(userId);
            return CustomMapper<IQueryable<Trip>, List<TripDTO>>.Map(rawTrips);
        }

        public List<TripDTO> GetHistoryOfTripsByUserId(int userId)
        {

            IQueryable<Trip> rawTrips = _unitOfWork.Trips.GetTripsByUserId(userId)
                .Where(el => el.EndTime < DateTime.UtcNow);
            return CustomMapper<IQueryable<Trip>, List<TripDTO>>.Map(rawTrips);
        }

        public List<TripDTO> GetTripsByUserId(int userId)
        {
            IQueryable<Trip> rawTrips = _unitOfWork.Trips.GetTripsByUserId(userId);
            return CustomMapper<IQueryable<Trip>, List<TripDTO>>.Map(rawTrips);
        }

        public IQueryable<int> GetYearsOfUserTrips(int userId)
        {
            return _unitOfWork.Trips.GetYearsOfUserTrips(userId);
        }

        public async Task<List<DurationInMonth>> GetTotalDurationByMonthsAsync(int year, int userId)
        {
            List<Trip> trips = await _unitOfWork.Trips.GetTripsByUserId(userId).ToListAsync();

            List<DurationInMonth> result = Enumerable.Range(1, 12)
                .Select(month => new DurationInMonth()
                {
                    Month = new DateTime(year, month, 1).ToString("MMMM"),
                    TotalDuration = trips
                        .Where(t => t.StartTime.Year == year && t.StartTime.Month <= month && t.EndTime.Year == year && t.EndTime.Month >= month)
                        .Select(t =>
                        {
                            var start = t.StartTime <= new DateTime(year, month, 1) ? new DateTime(year, month, 1) : t.StartTime;
                            DateTime end = t.EndTime >= new DateTime(year, month, DateTime.DaysInMonth(year, month)) ? new DateTime(year, month, DateTime.DaysInMonth(year, month)) : t.EndTime;
                            return (end - start).TotalHours;
                        })
                    .DefaultIfEmpty(0)
                    .Sum()
                }).ToList();
            return result;
        }
    }
}
