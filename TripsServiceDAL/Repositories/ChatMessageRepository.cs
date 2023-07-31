using TripsServiceDAL.Entities;
using TripsServiceDAL.Infrastructure;
using TripsServiceDAL.Interfaces;

namespace TripsServiceDAL.Repos;

public class ChatMessageRepository : EFGenericRepository<ChatMessage>, IChatMessageRepository
{
    public ChatMessageRepository(TripsDBContext context) : base(context)
    {
    }
}

