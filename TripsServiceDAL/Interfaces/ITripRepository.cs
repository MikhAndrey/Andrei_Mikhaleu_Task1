using TripsServiceDAL.Entities;

namespace TripsServiceDAL.Interfaces
{
    public interface ITripRepository : IGenericRepository<Trip>
    {
        IQueryable<int> GetYearsOfUserTrips(int userId);

        IQueryable<Trip> GetUserTripsWithUser(int userId);

        IQueryable<Trip> GetTripsByUserId(int userId);

        IQueryable<Trip> GetOthersPublicTrips(int userId);
    }
}
