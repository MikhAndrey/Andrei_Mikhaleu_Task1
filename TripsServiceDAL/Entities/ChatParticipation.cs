using TripsServiceDAL.Interfaces;

namespace TripsServiceDAL.Entities;

public class ChatParticipation: IIdentifiable, ISoftDelete
{
    public int Id { get; set; }
    public int ChatId { get; set; }
    public Chat Chat { get; set; }
    public int? UserId { get; set; }
    public User? User { get; set; }
    public List<ChatMessage> ChatMessages { get; set; }
    public bool IsDeleted { get; set; }
}
