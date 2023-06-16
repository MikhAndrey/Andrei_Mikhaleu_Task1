using TripsServiceDAL.Entities;
using TripsServiceDAL.Infrastructure;
using TripsServiceDAL.Interfaces;

namespace TripsServiceDAL.Repos
{
    internal class FeedbackRepository : EFGenericRepository<Feedback>, IFeedbackRepository
    {
        public FeedbackRepository(TripsDBContext context) : base(context) { }
    }
}
