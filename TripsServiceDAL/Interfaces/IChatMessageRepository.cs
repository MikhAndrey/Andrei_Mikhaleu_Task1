using TripsServiceDAL.Entities;

namespace TripsServiceDAL.Interfaces;

public interface IChatMessageRepository : IGenericRepository<ChatMessage>
{
    IQueryable<ChatMessage> GetByChatId(int chatId);
}
