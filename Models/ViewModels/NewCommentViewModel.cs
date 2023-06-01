using System.ComponentModel.DataAnnotations;

namespace Andrei_Mikhaleu_Task1.Models.ViewModels
{
    public class NewCommentViewModel
    {
        public int TripId { get; set; }

        [Required(ErrorMessage = "Comment message can't be empty")]
        public string Message { get; set; }
    }
}
