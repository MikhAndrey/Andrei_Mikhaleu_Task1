using TripsServiceDAL.Interfaces;

namespace TripsServiceDAL.Entities;

public class ChatMessage : IIdentifiable, ISoftDelete
{
    public int Id { get; set; }
    public string? Text { get; set; }
    public int ChatParticipationId { get; set; }
    public ChatParticipation ChatParticipation { get; set; }
    public bool IsDeleted { get; set; }
}
