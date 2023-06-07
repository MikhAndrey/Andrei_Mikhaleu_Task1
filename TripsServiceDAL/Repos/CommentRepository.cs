using Microsoft.EntityFrameworkCore;
using TripsServiceDAL.Infrastructure;
using TripsServiceDAL.Entities;

namespace TripsServiceDAL.Repos
{
    public class CommentRepository : EFGenericRepository<Comment>
    {
        public CommentRepository(TripsDBContext context) : base(context) { }

        public new async Task<Comment?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(c => c.User)
                .Include(c => c.Trip)
                .FirstOrDefaultAsync(c => c.CommentId == id);
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
