using System.ComponentModel.DataAnnotations;

namespace Andrei_Mikhaleu_Task1.Models.ViewModels
{
    public class NewTripViewModel
    {
        [Required(ErrorMessage = "Please enter a name for your trip")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please select the start time of your trip")]
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public bool Public { get; set; }

        public string Description { get; set; }

        public decimal Distance { get; set; }

        public int StartTimeZoneOffset { get; set; }

        public int FinishTimeZoneOffset { get; set; }
    }
}
