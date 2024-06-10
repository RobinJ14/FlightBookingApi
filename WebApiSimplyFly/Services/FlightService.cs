using WebApiSimplyFly.DTO;
using WebApiSimplyFly.Exceptions;
using WebApiSimplyFly.Interfaces;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Services
{
    public class FlightService : IFlightService
    {
        private readonly IRepository<Flight,int> _flightRepository;
        private readonly ILogger<FlightService> _logger;

     
        public FlightService(IRepository<Flight,int> flightRepository, ILogger<FlightService> logger)
        {
            _flightRepository = flightRepository;
            _logger = logger;
        }
        public async Task<Flight> AddFlight( Flight flight)
        {
            try
            {
                var flights = await _flightRepository.GetAsync(flight.FlightId);
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

        public async Task<Flight> GetFlightById(int id)
        {
            var flights = await _flightRepository.GetAsync(id);
            if (flights != null)

            {
                return flights;
            }
            throw new NoSuchFlightException();
        }

        public async Task<Flight> RemoveFlight(int flightNumber)
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

        public async Task<Flight> UpdateFlightDetails(int flightNumber, UpdateFlightDTO updateflight)
        {
            var flight = await _flightRepository.GetAsync(flightNumber);
            if (flight != null)
            {
                flight.FlightName = updateflight.FlightName;
                flight.TotalSeats = updateflight.TotalSeats;
                flight.BasePrice = updateflight.BasePrice;
                flight.BaggageCheckinWeight = updateflight.BaggageCheckinWeight;
                flight.BaggageCabinWeight = updateflight.BaggageCabinWeight;

                flight = await _flightRepository.Update(flight);
                _logger.LogInformation("Flight updated from service method");
                return flight;
            }
            throw new NoSuchFlightException();
        }

        public async Task<List<Flight>> GetFlightByFlightOwner(int ownerId)
        {
            var flights = await _flightRepository.GetAsync();
            if (flights != null)
            {
                var selectedFlight = flights.Where(e => e.OwnerId == ownerId).ToList();
                return selectedFlight;
            }
            throw new NoSuchFlightException();
        }
    }
}
