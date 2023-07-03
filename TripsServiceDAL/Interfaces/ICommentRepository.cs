using TripsServiceDAL.Entities;

namespace TripsServiceDAL.Interfaces;

public interface ICommentRepository : IGenericRepository<Comment>
{
	IQueryable<Comment> GetByTripId(int tripId);
}