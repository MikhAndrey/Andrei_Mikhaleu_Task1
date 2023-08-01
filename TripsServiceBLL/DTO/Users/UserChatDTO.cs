using TripsServiceBLL.DTO.Chats;

namespace TripsServiceBLL.DTO.Users;

public class UserChatDTO : UserListDTO
{
    public List<ChatMessageDTO> ChatMessages { get; set; }
}
