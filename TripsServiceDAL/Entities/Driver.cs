using System.ComponentModel.DataAnnotations.Schema;
using TripsServiceDAL.Interfaces;

namespace TripsServiceDAL.Entities;

public class Driver : IIdentifiable, ISoftDelete
{
    public string Name { get; set; }

    public List<DriverPhoto> Photos { get; set; }

    public int Experience { get; set; }

    public List<Trip> Trips { get; set; }

    [NotMapped]
    public double AverageRating => Math.Round(Trips.Where(t => t.Feedback != null)
        .Average(t => (double?) t.Feedback.Rating) ?? 0, 1);

    public int Id { get; set; }

    public bool IsDeleted { get; set; }
}