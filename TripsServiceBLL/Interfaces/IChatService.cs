using TripsServiceBLL.DTO.Chats;

namespace TripsServiceBLL.Interfaces;

public interface IChatService
{
    Task<ChatListDTO> AddAsync(ChatCreateDTO chat);
    IEnumerable<ChatListDTO> GetAll();
}
