using TripsServiceDAL.Entities;
using TripsServiceDAL.Infrastructure;
using TripsServiceDAL.Interfaces;

namespace TripsServiceDAL.Repos
{
    public class CommentRepository : EFGenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(TripsDBContext context) : base(context) { }

        public IQueryable<Comment> GetByTripId(int tripId)
        {
            return _dbSet.Where(i => i.TripId == tripId);
        }
    }
}
