namespace TripsServiceBLL.DTO.Chats;

public class ChatSendMessageDTO
{
    public int ChatId { get; set; }
    public int ChatParticipationId { get; set; }
    public string Text { get; set; }
}
