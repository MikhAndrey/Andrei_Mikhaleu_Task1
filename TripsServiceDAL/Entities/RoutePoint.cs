using System.ComponentModel.DataAnnotations.Schema;
using TripsServiceDAL.Interfaces;

namespace TripsServiceDAL.Entities;

public class RoutePoint : IIdentifiable, ISoftDelete
{
    [Column(TypeName = "decimal(12,9)")] public decimal Longitude { get; set; }

    [Column(TypeName = "decimal(12,9)")] public decimal Latitude { get; set; }

    public int Ordinal { get; set; }

    public int TripId { get; set; }

    public Trip? Trip { get; set; }
    public int Id { get; set; }

    public bool IsDeleted { get; set; }
}