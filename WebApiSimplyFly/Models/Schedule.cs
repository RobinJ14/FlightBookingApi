using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiSimplyFly.Models
{
    public class Schedule
    {
        [Key]
        public int ScheduleId { get; set; }

        public int RouteId { get; set; }
        [ForeignKey("RouteId")]
        public Route? Route { get; set; }
        public int FlightId { get; set; }
        [ForeignKey("FlightId")]
        public Flight? Flight { get; set; }

        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}
