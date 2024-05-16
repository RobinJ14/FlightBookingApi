using Microsoft.EntityFrameworkCore;
using WebApiSimplyFly.Context;
using WebApiSimplyFly.Exceptions;
using WebApiSimplyFly.Interfaces;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Repositories
{
    public class FlightRepository : IRepository<Flight, string>
    {
        private readonly newContext _context;
        private readonly ILogger<FlightRepository> _logger;

        
        public FlightRepository(newContext context, ILogger<FlightRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Flight> Add(Flight items)
        {
            _context.Add(items);
            _context.SaveChanges();
            _logger.LogInformation("Flight added " + items.FlightNo);
            return items;
        }

        public async Task<Flight> Delete(string key)
        {
            var flight = await GetAsync(key);
            if (flight != null)
            {
                _context.Remove(flight);
                _context.SaveChanges();
                _logger.LogInformation("Flight deleted with flight number" + key);
                return flight;
            }
            throw new NoSuchFlightException();
        }

        public async Task<Flight> GetAsync(string key)
        {
            var flights = await GetAsync();
            var flight = flights.FirstOrDefault(e => e.FlightNo == key);
            if (flight != null)
            {
                return flight;
            }
            throw new NoSuchFlightException();
        }

        public async Task<List<Flight>> GetAsync()
        {
            var flights = _context.Flights.ToList();
            return flights;
        }

        public async Task<Flight> Update(Flight items)
        {
            var flight = await GetAsync(items.FlightNo);
            if (flight != null)
            {
                _context.Entry<Flight>(items).State = EntityState.Modified;
                _context.SaveChanges();
                _logger.LogInformation("Flight updated with flight number" + items.FlightNo);
                return flight;
            }
            throw new NoSuchFlightException();
        }
    }
}
