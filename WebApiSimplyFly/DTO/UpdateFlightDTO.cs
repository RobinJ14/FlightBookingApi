namespace WebApiSimplyFly.DTO
{
    public class UpdateFlightDTO
    {
        public string FlightName { get; set; } = string.Empty;
        public int TotalSeats { get; set; }
        public double BasePrice { get; set; }
        public double BaggageCheckinWeight { get; set; }
        public double BaggageCabinWeight { get; set; }
    }
}
