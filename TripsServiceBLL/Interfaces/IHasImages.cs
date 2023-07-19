using TripsServiceBLL.DTO.Images;

namespace TripsServiceBLL.Interfaces;

public interface IHasImages
{
    List<ImageDTO> Images { get; set; }
}
