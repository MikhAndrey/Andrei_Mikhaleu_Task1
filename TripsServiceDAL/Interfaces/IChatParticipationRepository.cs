using TripsServiceDAL.Entities;

namespace TripsServiceDAL.Interfaces;

public interface IChatParticipationRepository: IGenericRepository<ChatParticipation>
{
    Task<ChatParticipation?> GetEmptyChatParticipation(int chatId);
    Task<ChatParticipation?> GetByChatIdAndUserId(int chatId, int? userId);
}
