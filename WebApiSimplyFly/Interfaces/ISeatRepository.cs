using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Interfaces
{
    public interface ISeatRepository
    {
        Task<IEnumerable<Seat>> GetSeatDetailsAsync(List<string> seatNos);
        Task UpdateSeatDetailsAsync(IEnumerable<Seat> seatDetails);
        Task<List<Seat>> GetSeatDetailsByFlight(string FlightNo);

    }
}
