namespace Andrei_Mikhaleu_Task1.Models.Entities
{
    public class Image
    {
        public int ImageId { get; set; }

        public byte[] Data { get; set; }

        public int TripId { get; set; }

        public Trip Trip { get; set; }

    }
}
