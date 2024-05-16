using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiSimplyFly.Models
{
    public class Refund
    {
        public int RefundId { get; set; }
        public int BookingId { get; set; }
        [ForeignKey("BookingId")]
        public Booking Booking { get; set; }
        public DateTime RefundDate { get; set; }
        public double RefundAmount { get; set; }

        public RefundStatus Status { get; set; }

        public enum RefundStatus
        {
            Successful,
            Failed,
            RefundIssued
        }
    }
}
