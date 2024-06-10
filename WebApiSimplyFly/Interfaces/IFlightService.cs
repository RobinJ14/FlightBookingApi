using WebApiSimplyFly.DTO;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Interfaces
{
    public interface IFlightService
    {
        public Task<Flight> AddFlight(Flight flight);
        public Task<Flight> RemoveFlight(int flightNumber);
        public Task<List<Flight>> GetAllFlights();
        public Task<Flight> GetFlightById(int id);
        public Task<List<Flight>> GetFlightByFlightOwner(int ownerId);
        public Task<Flight> UpdateFlightDetails(int flightNumber, UpdateFlightDTO updateflight);

       
    }
}
