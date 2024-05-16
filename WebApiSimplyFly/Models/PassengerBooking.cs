using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiSimplyFly.Models
{
    public class PassengerBooking
    {
        [Key]
        public int PassengerBookingId { get; set; }
        public int BookingId { get; set; }
        [ForeignKey("BookingId")]
        public Booking? Booking { get; set; }

        public int PassengerId { get; set; }
        [ForeignKey("PassengerId")]
        public Passenger? Passenger { get; set; }

        public string SeatNo { get; set; }
        [ForeignKey("SeatNo")]
        public Seat Seat { get; set; }
    }
}
