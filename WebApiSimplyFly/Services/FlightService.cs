using WebApiSimplyFly.Exceptions;
using WebApiSimplyFly.Interfaces;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Services
{
    public class FlightService : IFlightService
    {
        private readonly IRepository<Flight,string> _flightRepository;
        private readonly ILogger<FlightService> _logger;

     
        public FlightService(IRepository<Flight,string> flightRepository, ILogger<FlightService> logger)
        {
            _flightRepository = flightRepository;
            _logger = logger;
        }
        public async Task<Flight> AddFlight(Flight flight)
        {
            try
            {
                var flights = await _flightRepository.GetAsync(flight.FlightNo);
                throw new FlightAlreadyPresentException();
            }
            catch (NoSuchFlightException)
            {
                flight = await _flightRepository.Add(flight);
                _logger.LogInformation("Flight added from service method");
                return flight;
            }
        }

        public async Task<List<Flight>> GetAllFlights()
        {
            var flights = await _flightRepository.GetAsync();
            return flights;
        }

        public async Task<Flight> GetFlightById(string id)
        {
            var flights = await _flightRepository.GetAsync(id);
            if (flights != null)

            {
                return flights;
            }
            throw new NoSuchFlightException();
        }

        public async Task<Flight> RemoveFlight(string flightNumber)
        {
            var flight = await _flightRepository.GetAsync(flightNumber);
            if (flight != null)
            {
                flight = await _flightRepository.Delete(flightNumber);
                _logger.LogInformation("Flight removed from service method");
                return flight;
            }
            throw new NoSuchFlightException();
        }

        public async Task<Flight> UpdateAirline(string flightNumber, string airline)
        {
            var flight = await _flightRepository.GetAsync(flightNumber);
            if (flight != null)
            {
                flight.FlightName = airline;
                flight = await _flightRepository.Update(flight);
                _logger.LogInformation("Flight updated from service method");
                return flight;
            }
            throw new NoSuchFlightException();
        }

        public async Task<Flight> UpdateTotalSeats(string flightNumber, int totalSeats)
        {
            var flight = await _flightRepository.GetAsync(flightNumber);
            if (flight != null)
            {
                flight.TotalSeats = totalSeats;
                flight = await _flightRepository.Update(flight);
                _logger.LogInformation("Flight updated from service method");
                return flight;
            }
            throw new NoSuchFlightException();
        }
    }
}
