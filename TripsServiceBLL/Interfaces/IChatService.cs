using TripsServiceBLL.DTO.Chats;
using TripsServiceDAL.Entities;

namespace TripsServiceBLL.Interfaces;

public interface IChatService
{
    Task AddChatAsync(Chat chat);
    Task DeleteWithoutSavingAsync(int id);
    Task DeleteChatMessagesWithoutSavingAsync(int id);
    Task DeleteChatParticipationsWithoutSavingAsync(int id);
    Task AddChatParticipationAsync(ChatParticipation chatParticipation);
    Task AddChatMessageAsync(ChatMessage chatMessage);
    IEnumerable<ChatListDTO> GetAll();
    Task<ChatDetailsDTO> GetByIdAsync(int id);
    ChatMessage CreateMessageAboutChatJoining(int chatParticipationId);
    ChatMessage CreateMessageAboutChatLeaving(int chatParticipationId);
    Task<int?> GetEmptyChatParticipationIdAsync(int chatId);
    Task<int> GetCurrentChatParticipationIdAsync(int chatId);
    Task<ChatMessageDTO> SendMessageAsync(ChatSendMessageDTO dto);
    Task<ChatMessageDTO> LeaveChatAsync(ChatLeaveDTO dto);
    Task ActivateChatParticipationAsync(ChatParticipation chatParticipation);
    Task DeactivateChatParticipationAsync(ChatParticipation chatParticipation);
}
