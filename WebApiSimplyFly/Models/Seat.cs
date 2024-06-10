using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiSimplyFly.Models
{
    public class Seat
    {
        [Key]
        public int SeatId { get; set; }
        public string SeatClass { get; set; } = string.Empty;
        public int FlightId { get; set; } 
        [ForeignKey("FlightId")]

        public Flight Flight { get; set; }


    }
}
