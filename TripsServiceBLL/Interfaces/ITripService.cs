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

		Task DeleteAsync(int tripId);

		Task AddAsync(Trip trip);

		Task<TripDetailsDTO> GetTripDetailsAsync(int tripId);

		Task<EditTripDTO> GetTripForEditingAsync(int tripId);

		IQueryable<ReadTripDTOExtended> GetOthersPublicTrips();

		IQueryable<ReadTripDTO> GetCurrentUserHistoryOfTrips();

		IQueryable<ReadTripDTO> GetCurrentUserTrips();

		YearsStatisticsDTO GetYearsOfCurrentUserTrips();

		Task<List<UtilDurationInMonth>> GetTotalDurationByMonthsAsync(int year);

		Task<EditPastTripDTO> GetPastTripForEditingAsync(int id);
	}
}
