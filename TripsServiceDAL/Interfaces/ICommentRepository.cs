using TripsServiceDAL.Entities;

namespace TripsServiceDAL.Interfaces
{
	public interface ICommentRepository : IGenericRepository<Comment>
	{
		IQueryable<Comment> GetCommentsByTripId(int tripId);
	}
}
