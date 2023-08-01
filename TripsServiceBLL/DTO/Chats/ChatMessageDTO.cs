namespace TripsServiceBLL.DTO.Chats;

public class ChatMessageDTO
{
    public int Id { get; set; }
    public string Text { get; set; }
    public int ChatParticipationId { get; set; }
}
