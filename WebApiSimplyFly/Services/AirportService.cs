using WebApiSimplyFly.Exceptions;
using WebApiSimplyFly.Interfaces;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Services
{
    public class AirportService :IAirportService
    {
        private readonly ILogger<RouteService> _logger;
        private readonly IRepository<Airport, int> _airportRepository;

        public AirportService( ILogger<RouteService> logger, IRepository<Airport, int> airportRepository)
        {
            _airportRepository = airportRepository;
            _logger = logger;

        }

        public async Task<Airport> AddAirport(Airport airport)
        {
            var airports = await _airportRepository.GetAsync();
            var existingAirport = airports.FirstOrDefault(e => e.Name == airport.Name && e.City == airport.City);
            if (existingAirport == null)

            {
                airport = await _airportRepository.Add(airport);
                return airport;
            }
            throw new AirportAlreadyPresentException();
        }

        public async Task<List<Airport>> GetAllAirports()
        {
            var airports = await _airportRepository.GetAsync();
            return airports;
        }

        public async Task<Airport> GetByIdAirport(int id)
        {
            var route = await _airportRepository.GetAsync(id);

            if (route != null)
            {
                return route;
            }
            throw new NoSuchAirportException();
        }

        public async Task<bool> RemoveAirport(int Id)
        {
            if (await _airportRepository.Delete(Id) != null)
            {
                return true;
            };
            return false;
        }

        public async Task<Airport> UpdateAirport(Airport airport)
        {
            var updateAirport = await _airportRepository.GetAsync(airport.Id);
            if (updateAirport != null)
            {
                updateAirport.Name = airport.Name;
                updateAirport.City = airport.City;
                updateAirport.State = airport.State;
                updateAirport.Country = airport.Country;


                updateAirport = await _airportRepository.Update(updateAirport);
                return updateAirport;
            }
            throw new NoSuchAirportException();
        }
    }
}

