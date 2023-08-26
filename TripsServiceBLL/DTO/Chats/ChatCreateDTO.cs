using System.ComponentModel.DataAnnotations;

namespace TripsServiceBLL.DTO.Chats;

public class ChatCreateDTO
{
    [Required(ErrorMessage = "Please, enter a non-empty name of chat")]
    public string Name { get; set; }
}
