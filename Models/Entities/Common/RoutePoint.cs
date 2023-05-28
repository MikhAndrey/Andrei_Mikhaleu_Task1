using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Andrei_Mikhaleu_Task1.Models.Entities.Common
{
    public class RoutePoint
    {
        public RoutePoint()
        {
            Trip = new();
        }

        public int RoutePointId { get; set; }

        [Column(TypeName = "decimal(12,9)")]
        public decimal Longitude { get; set; }

        [Column(TypeName = "decimal(12,9)")]
        public decimal Latitude { get; set; }

        public int Ordinal { get; set; }

        public int TripId { get; set; }

        public Trip Trip { get; set; }
    }
}
