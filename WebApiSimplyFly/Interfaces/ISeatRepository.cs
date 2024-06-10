using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Interfaces
{
    public interface ISeatRepository
    {
        Task<IEnumerable<Seat>> GetSeatDetailsAsync(List<int> seatNos);
        Task<bool> AddSeatDetailsAsync(IEnumerable<Seat> seatDetails);
        Task<List<Seat>> GetSeatDetailsByFlight(int FlightNo);
        Task<bool> DeleteSeatByFlightId(int flightId);


    }
}
