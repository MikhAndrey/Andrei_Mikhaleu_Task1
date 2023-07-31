using TripsServiceDAL.Entities;
using TripsServiceDAL.Infrastructure;
using TripsServiceDAL.Interfaces;

namespace TripsServiceDAL.Repos;

public class ChatParticipationRepository : EFGenericRepository<ChatParticipation>, IChatParticipationRepository
{
    public ChatParticipationRepository(TripsDBContext context) : base(context)
    {
    }
}
