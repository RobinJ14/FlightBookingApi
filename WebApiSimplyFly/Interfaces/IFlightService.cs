using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Interfaces
{
    public interface IFlightService
    {
        public Task<Flight> AddFlight(Flight flight);
        public Task<Flight> RemoveFlight(string flightNumber);
        public Task<List<Flight>> GetAllFlights();
        public Task<Flight> GetFlightById(string id);
        public Task<Flight> UpdateAirline(string flightNumber, string airline);
        public Task<Flight> UpdateTotalSeats(string flightNumber, int totalSeats);
    }
}
