using Microsoft.EntityFrameworkCore;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Infrastructure;
using TripsServiceDAL.Interfaces;

namespace TripsServiceDAL.Repos;

public class ChatRepository : EFGenericRepository<Chat>, IChatRepository
{
    public ChatRepository(TripsDBContext context) : base(context)
    {
    }

    public new Task<Chat> GetByIdAsync(int id)
    {
        return _dbSet
            .Include(c => c.ChatParticipations)
            .ThenInclude(chp => chp.User)
            .Include(c => c.ChatParticipations)
            .ThenInclude(chp => chp.ChatMessages).FirstOrDefaultAsync(c => c.Id == id);
    }
}
