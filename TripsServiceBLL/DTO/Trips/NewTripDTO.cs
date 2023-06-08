namespace TripsServiceBLL.DTO.Trips
{
    public class NewTripDTO
    {
        public string Name { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public bool Public { get; set; }

        public string Description { get; set; }

        public decimal Distance { get; set; }

        public int StartTimeZoneOffset { get; set; }

        public int FinishTimeZoneOffset { get; set; }
    }
}
