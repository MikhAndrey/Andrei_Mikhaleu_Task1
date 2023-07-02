using Microsoft.EntityFrameworkCore;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Infrastructure;
using TripsServiceDAL.Interfaces;

namespace TripsServiceDAL.Repos;

internal class FeedbackRepository : EFGenericRepository<Feedback>, IFeedbackRepository
{
    public FeedbackRepository(TripsDBContext context) : base(context)
    {
    }

    public async Task<Feedback> GetByTripId(int tripId)
    {
        Feedback? feedback = await _dbSet.FirstOrDefaultAsync(f => f.TripId == tripId);
        ThrowErrorIfEntityIsNull(feedback);
        return feedback;
    }
}