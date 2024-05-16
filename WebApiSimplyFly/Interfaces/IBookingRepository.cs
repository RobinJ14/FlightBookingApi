using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Interfaces
{
    public interface IBookingRepository
    {
        Task<IEnumerable<Booking>> GetBookingsByCustomerIdAsync(int CustomerId);

    }
}
