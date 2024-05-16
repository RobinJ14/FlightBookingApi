using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiSimplyFly.Models
{
    public class FlightOwner
    {
        [Key]
        public int OwnerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string BusinessRegistrationNumber { get; set; } = string.Empty;
        public string Username { get; set; }
        [ForeignKey("Username")]
        public User User { get; set; }
        // Navigation Properties
        public List<Flight>? OwnedFlights { get; set; }
    }
}
