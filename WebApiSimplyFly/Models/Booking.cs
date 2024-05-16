using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiSimplyFly.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }
        public int ScheduleId { get; set; }
        [ForeignKey("ScheduleId")]
        public Schedule? Schedule { get; set; }

        public int PaymentId { get; set; }
        [ForeignKey("PaymentId")]
        public Payment? Payment { get; set; }
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; } 
        public DateTime BookingTime { get; set; }

        public BookingStatus bookingStatus { get; set; }
        public int SeatCount { get; set; }
        public double TotalPrice { get; set; }

        public enum BookingStatus
        {
            Successful,
            Failed,
            Cancelled
        }
    }
}
