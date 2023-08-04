using TripsServiceBLL.DTO.Users;

namespace TripsServiceBLL.DTO.Chats;

public class ChatJoinDTO
{
    public UserChatDTO User { get; set; }
    public ChatMessageDTO Message { get; set; }
}
