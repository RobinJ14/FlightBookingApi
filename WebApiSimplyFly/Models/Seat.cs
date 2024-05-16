using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiSimplyFly.Models
{
    public class Seat
    {
        [Key]
        public string SeatNo { get; set; } = string.Empty;
        public string SeatClass { get; set; } = string.Empty;
        public string FlightNo { get; set; } = string.Empty;
        [ForeignKey("FlightNo")]

        public Flight Flight { get; set; }


    }
}
