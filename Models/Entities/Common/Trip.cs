using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Andrei_Mikhaleu_Task1.Models.Entities.Common
{
    public class Trip
    {
        public Trip()
        {
            RoutePoints = new();
            Images = new();
            Comments = new();
            User = new();
        }

        public int TripId { get; set; }

        public string Name { get; set; }

        public virtual List<RoutePoint> RoutePoints { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        [Column(TypeName = "decimal(10,3)")]
        public decimal Distance { get; set; }

        public string Description { get; set; }

        public bool Public { get; set; }

        public List<Image> Images { get; set; }

        public List<Comment> Comments { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public int StartTimeZoneOffset { get; set; }

        public int FinishTimeZoneOffset { get; set; }
    }
}
