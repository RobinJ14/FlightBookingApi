using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiSimplyFly.Models
{
    public class Flight
    {
        [Key]
        public int FlightId { get; set; }
        public string FlightName { get; set; } = string.Empty;
        public int TotalSeats { get; set; }
        public double BasePrice { get; set; }
        public double BaggageCheckinWeight { get; set; }
        public double BaggageCabinWeight { get; set; }
        public int OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public FlightOwner? FlightOwner { get; set; }

    }
}
