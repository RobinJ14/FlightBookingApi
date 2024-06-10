namespace WebApiSimplyFly.DTO
{
    public class AllScheduleDTO
    {
        public int Id { get; set; }
        public int FlightId { get; set; }
        public string SourceAirport { get; set; }
        public string DestinationAirport { get; set; }
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }
    }
}
