using System.Threading.Tasks;
using TripsServiceBLL.DTO.Statistics;
using TripsServiceBLL.DTO.Trips;
using TripsServiceBLL.Utils;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Interfaces
{
    public interface ITripService
    {
        Task<Trip?> GetByIdAsync(int id);

        void SetNewTimeForStartingTrip(Trip trip);

        void SetNewTimeForEndingTrip(Trip trip);

        Task UpdateAsync(Trip trip);

        Task DeleteAsync(Trip trip);

        Task AddAsync(Trip trip);

        Task<TripDTO> InitializeTripDTOAsync(int tripId, int userId);

        Task<ExtendedExistingTripDTO> InitializeExtendedExistingTripAsync(int tripId);

        List<TripDTO> GetOthersPublicTrips(int userId);

        List<TripDTO> GetHistoryOfTripsByUserId(int userId);

        List<TripDTO> GetTripsByUserId(int userId);

        YearsStatisticsDTO GetYearsOfUserTrips(int userId);

        Task<List<DurationInMonth>> GetTotalDurationByMonthsAsync(int year, int userId);
    }
}
