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
        return await GetByChatIdAndUserId(chatId, null);
    }

    public async Task<ChatParticipation?> GetByChatIdAndUserId(int chatId, int? userId)
    {
        ChatParticipation? chatParticipation = await _dbSet
            .FirstOrDefaultAsync(cp => cp.ChatId == chatId && cp.UserId == userId);
        ThrowErrorIfEntityIsNull(chatParticipation);
        return chatParticipation;
    }

    public IQueryable<ChatParticipation> GetByChatId(int chatId)
    {
        return _dbSet.Where(chp => chp.ChatId == chatId);
    }
}
