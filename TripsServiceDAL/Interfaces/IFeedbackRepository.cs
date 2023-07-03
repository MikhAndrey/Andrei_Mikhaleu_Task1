using TripsServiceDAL.Entities;

namespace TripsServiceDAL.Interfaces;

public interface IFeedbackRepository : IGenericRepository<Feedback>
{
	Task<Feedback> GetByTripId(int tripId);
}