namespace Andrei_Mikhaleu_Task1.Models.Entities
{
    public class Image
    {
        public Image() 
        {
            Trip = new();
        }
        
        public int ImageId { get; set; }

        public string Link { get; set; }

        public int TripId { get; set; }

        public Trip Trip { get; set; }

    }
}
