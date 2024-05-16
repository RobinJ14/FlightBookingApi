using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiSimplyFly.Models
{
    public class History
    {
        public int HistoryId { get; set; }

        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        public int BookingId { get; set; }
        [ForeignKey("BookingId")]
        public Booking Booking { get; set; }
        public ActionByCustomer Action { get; set; }
        public DateTime ActionDate { get; set; }

        public enum ActionByCustomer
        {
            BookingSuccessful,
            BookingCancelled,

        }


    }
}
