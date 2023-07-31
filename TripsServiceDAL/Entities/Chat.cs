using TripsServiceDAL.Interfaces;

namespace TripsServiceDAL.Entities;

public class Chat: IIdentifiable, ISoftDelete
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<ChatParticipation> ChatParticipations { get; set; }
    public bool IsDeleted { get; set; }
}
