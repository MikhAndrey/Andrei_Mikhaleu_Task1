using TripsServiceBLL.DTO.Trips;
using TripsServiceBLL.DTO.Users;

namespace TripsServiceBLL.DTO.Comments;

public class CommentDTO : CreateCommentDTO
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int UserId { get; set; }

    public UserDTO? User { get; set; }

    public ReadTripDTO? Trip { get; set; }
}