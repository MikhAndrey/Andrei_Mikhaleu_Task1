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

    public new async Task<Chat> GetByIdAsync(int id)
    {
        Chat? chat = await _dbSet
            .Include(c => c.ChatParticipations)
            .ThenInclude(chp => chp.User)
            .ThenInclude(u => u.Role)
            .Include(c => c.ChatParticipations)
            .ThenInclude(chp => chp.ChatMessages)
            .FirstOrDefaultAsync(c => c.Id == id);
        return chat;
    }

    public async Task<Chat> GetByIdForAddingUserAsync(int chatId)
    {
        Chat? chat = await _dbSet
            .Include(c => c.ChatParticipations)
            .FirstOrDefaultAsync(c => c.Id == chatId);
        ThrowErrorIfEntityIsNull(chat);
        return chat;
    }
}
