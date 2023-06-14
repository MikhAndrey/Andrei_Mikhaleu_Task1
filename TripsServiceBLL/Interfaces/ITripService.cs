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

		Task EndTripAsync(int tripId);

		Task StartTripAsync(int tripId);

		Task UpdateAsync(Trip trip);

		Task DeleteAsync(Trip trip);

		Task AddAsync(Trip trip);

		Task<TripDetailsDTO> GetTripDetailsAsync(int tripId, int userId);

		Task<EditTripDTO> GetTripForEditingAsync(int tripId);

		IQueryable<ReadTripDTOExtended> GetOthersPublicTrips(int userId);

		IQueryable<ReadTripDTO> GetHistoryOfTripsByUserId(int userId);

		IQueryable<ReadTripDTO> GetTripsByUserId(int userId);

		YearsStatisticsDTO GetYearsOfUserTrips(int userId);

		Task<List<DurationInMonth>> GetTotalDurationByMonthsAsync(int year, int userId);
	}
}
