using System.ComponentModel.DataAnnotations;
using TripsServiceBLL.Interfaces;

namespace TripsServiceBLL.DTO.Trips
{
    public class CreateTripDTO : IMinimalTripFactory
    {
        [Required(ErrorMessage = "Please enter a name for your trip")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Please select the start time of your trip")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "This field will be filled automatically when you build the route of at least two points")]
        public DateTime EndTime { get; set; }

        public bool Public { get; set; }

        public string? Description { get; set; }

        public decimal Distance { get; set; }

        public int StartTimeZoneOffset { get; set; }

        public int FinishTimeZoneOffset { get; set; }

        public int? DriverId { get; set; }
    }
}
