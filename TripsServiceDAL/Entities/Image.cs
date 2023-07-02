using TripsServiceDAL.Interfaces;

namespace TripsServiceDAL.Entities;

public class Image : IIdentifiable, ISoftDelete
{
    public string? Link { get; set; }

    public int TripId { get; set; }

    public Trip? Trip { get; set; }
    public int Id { get; set; }

    public bool IsDeleted { get; set; }
}