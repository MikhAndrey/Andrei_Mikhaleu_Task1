using TripsServiceBLL.DTO.Users;

namespace TripsServiceBLL.DTO.Chats;

public class ChatDetailsDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<UserChatDTO> Users { get; set; }
    public List<ChatMessageDTO> Messages { get; set; }
    public bool IsCurrentUserInChat { get; set; }
}
