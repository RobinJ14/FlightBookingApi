using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiSimplyFly.Models
{
    public class Route
    {
        [Key]
        public int RouteId { get; set; }
        public int SourceAirportId { get; set; }
        [ForeignKey("SourceAirportId")]
        public Airport? SourceAirport { get; set; }
        public int DestinationAirportId { get; set; }
        [ForeignKey("DestinationAirportId")]
        public Airport? DestinationAirport { get; set; }
        public ICollection<Schedule>? Schedules { get; set; }
    }
}
