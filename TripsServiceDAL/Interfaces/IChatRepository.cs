using TripsServiceDAL.Entities;

namespace TripsServiceDAL.Interfaces;

public interface IChatRepository: IGenericRepository<Chat>
{
    Task<Chat> GetByIdForAddingUserAsync(int chatId);
}
