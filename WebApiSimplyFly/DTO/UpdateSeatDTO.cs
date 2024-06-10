namespace WebApiSimplyFly.DTO
{
    public class UpdateSeatDTO
    {
        public int FlightId { get; set; }
        public int EconomyCount { get; set; }
        public int PremiumEconomyCount { get; set; }
        public int PremiumCount { get; set; }
    }
}
