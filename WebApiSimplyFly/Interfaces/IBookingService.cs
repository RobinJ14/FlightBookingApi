using WebApiSimplyFly.DTO;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<List<Booking>> GetBookingBySchedule(int scheduleId);
        Task<bool> CreateBookingAsync(BookingRequestDTO bookingRequest);
        Task<Booking> CancelBookingAsync(int bookingId);
        Task<IEnumerable<Booking>> GetUserBookingsAsync(int userId);
        Task<Booking> GetBookingByIdAsync(int bookingId);
        Task<bool> RequestRefundAsync(int bookingId);
        Task<List<Booking>> GetBookingByFlight(string flightNumber);
        Task<List<string>> GetBookedSeatBySchedule(int scheduleID);
        Task<List<PassengerBooking>> GetBookingsByCustomerId(int customerId);
        Task<PassengerBooking> CancelBookingByPassenger(int passengerId);
    }
}
