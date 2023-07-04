using TripsServiceDAL.Entities;

namespace TripsServiceDAL.Interfaces;

public interface ITripRepository : IGenericRepository<Trip>
{
	IQueryable<int> GetYearsOfUserTrips(int userId);
	IQueryable<Trip> GetTripsByUserId(int userId);
	IQueryable<Trip> GetTripsByYearAndUserId(int year, int userId);
	IQueryable<Trip> GetOthersPublicTrips(int userId);
	IQueryable<Trip> GetHistoryOfTripsByUserId(int userId);
	Task<Trip> GetByIdForMinimalEditingAsync(int id);
	Task<Trip> GetByIdForEditingAsync(int id);
	Task<Trip> GetByIdForDetailsAsync(int id);
	Task<Trip> GetByIdWithImagesAsync(int id);
}
