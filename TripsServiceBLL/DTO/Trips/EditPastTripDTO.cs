using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using TripsServiceBLL.DTO.Images;
using TripsServiceBLL.Interfaces;

namespace TripsServiceBLL.DTO.Trips
{
    public class EditPastTripDTO : IMinimalTripChanges, IMinimalTripFactory
    {
        [Required(ErrorMessage = "Please enter a name for your trip")]
        public string? Name { get; set; }

        public bool Public { get; set; }

        public string? Description { get; set; }

        public List<ImageDTO> Images { get; set; }

        public int Id { get; set; }

        public int UserId { get; set; }

        public List<IFormFile>? ImagesAsFiles { get; set; }
    }
}
