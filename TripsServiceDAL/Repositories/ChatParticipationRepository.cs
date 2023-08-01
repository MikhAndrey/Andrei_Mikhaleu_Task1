using Microsoft.EntityFrameworkCore;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Infrastructure;
using TripsServiceDAL.Interfaces;

namespace TripsServiceDAL.Repos;

public class ChatParticipationRepository : EFGenericRepository<ChatParticipation>, IChatParticipationRepository
{
    public ChatParticipationRepository(TripsDBContext context) : base(context)
    {
    }

    public async Task<ChatParticipation?> GetEmptyChatParticipation(int chatId)
    {
        ChatParticipation? chatParticipation = await _dbSet
            .FirstOrDefaultAsync(cp => cp.ChatId == chatId && cp.UserId == null);
        return chatParticipation;
    }
}
