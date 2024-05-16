using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.DTO
{
    public class BookingRequestDTO
    {
        public int ScheduleId { get; set; }
        public int UserId { get; set; }
        public DateTime BookingTime { get; set; }
        public List<int> PassengerIds { get; set; }
        public List<string> SelectedSeats { get; set; }
        public double Price { get; set; }
        public PaymentDetails PaymentDetails { get; set; }
    }
}
