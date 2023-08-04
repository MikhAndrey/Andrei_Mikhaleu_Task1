using TripsServiceBLL.DTO.Users;

namespace TripsServiceBLL.DTO.Chats;

public class ChatSendMessageDTO
{
    public int ChatId { get; set; }
    public UserChatDTO User { get; set; }
    public string Text { get; set; }
}
