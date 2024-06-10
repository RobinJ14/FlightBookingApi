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
        Task<Booking> GetBookingByIdAsync(int bookingId);
        Task<bool> RequestRefundAsync(int bookingId);
        Task<List<Booking>> GetBookingByFlight(int flightNumber);
        Task<List<PassengerBooking>> GetBookingsByCustomerId(int customerId);
        Task<IEnumerable<Booking>> GetBookingByCustomer(int customerId);

        Task<PassengerBooking> CancelBookingByPassenger(int passengerId);

        Task<List<PassengerBooking>> GetPassengerBookingByBookingId(int bookingId);

    }
}
