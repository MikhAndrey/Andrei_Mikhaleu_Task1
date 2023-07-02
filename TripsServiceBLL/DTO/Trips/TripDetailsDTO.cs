using TripsServiceBLL.DTO.Comments;
using TripsServiceBLL.DTO.Drivers;
using TripsServiceBLL.DTO.Images;
using TripsServiceBLL.DTO.RoutePoints;

namespace TripsServiceBLL.DTO.Trips;

public class TripDetailsDTO : ReadTripDTO
{
    public int UserId { get; set; }

    public bool IsCurrentUserTrip { get; set; }

    public bool Public { get; set; }

    public List<ImageDTO> Images { get; set; }

    public List<RoutePointDTO> RoutePoints { get; set; }

    public List<CommentDTO> Comments { get; set; }

    public int StartTimeZoneOffset { get; set; }

    public int FinishTimeZoneOffset { get; set; }

    public string Duration { get; set; }

    public decimal Distance { get; set; }

    public ReadDriverDTO? Driver { get; set; }
}