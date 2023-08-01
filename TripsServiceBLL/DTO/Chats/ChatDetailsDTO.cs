using TripsServiceBLL.DTO.Users;

namespace TripsServiceBLL.DTO.Chats;

public class ChatDetailsDTO
{
    public string Name { get; set; }
    public List<UserChatDTO> Users { get; set; }
}
