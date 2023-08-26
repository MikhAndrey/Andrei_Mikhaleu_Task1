using TripsServiceBLL.DTO.Users;

namespace TripsServiceBLL.DTO.Chats;

public class ChatNotificationMessageDTO
{
    public int Id { get; set; }
    public int ChatId { get; set; }
    public string Text { get; set; }
    public string ChatName { get; set; }
    public UserChatDTO User { get; set; }
}
