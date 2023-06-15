using Microsoft.EntityFrameworkCore;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Infrastructure;
using TripsServiceDAL.Interfaces;

namespace TripsServiceDAL.Repos
{
	public class CommentRepository : EFGenericRepository<Comment>, ICommentRepository
	{
		public CommentRepository(TripsDBContext context) : base(context) { }

		public new async Task<Comment?> GetByIdAsync(int id)
		{
			return await _dbSet
				.Include(c => c.User)
				.Include(c => c.Trip)
				.FirstOrDefaultAsync(c => c.Id == id);
		}

		public IQueryable<Comment> GetCommentsByTripId(int tripId)
		{
			return _dbSet
				.Include(c => c.User)
				.Include(c => c.Trip)
				.Where(c => c.TripId == tripId);
		}
	}
}
