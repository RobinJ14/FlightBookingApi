using Microsoft.EntityFrameworkCore;
using WebApiSimplyFly.Context;
using WebApiSimplyFly.Exceptions;
using WebApiSimplyFly.Interfaces;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Repositories
{
    public class AirportRepository : IRepository<Airport, int>
    {
        private readonly newContext _context;
        private readonly ILogger<AirportRepository> _logger;

        public AirportRepository(newContext context, ILogger<AirportRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Airport> Add(Airport items)
        {
            _context.Add(items);
            _context.SaveChanges();
            _logger.LogInformation($"Airport added with id {items.Id}");
            return items;
        }

        public async  Task<Airport> Delete(int key)
        {
            var airport = await GetAsync(key);
            if (airport != null)
            {
                _context.Remove(airport);
                _context.SaveChanges();
                _logger.LogInformation($"Airport removed with id {key}");
                return airport;
            }
            throw new NoSuchAirportException();
        }

        public async Task<Airport> GetAsync(int key)
        {
            var airports = await GetAsync();
            var airport = airports.FirstOrDefault(e => e.Id == key);
            if (airport != null)
            {
                return airport;
            }
            throw new NoSuchAirportException();
        }

        public async Task<List<Airport>> GetAsync()
        {
            var airports = _context.Airports.ToList();
            return airports;
        }

        public async Task<Airport> Update(Airport items)
        {
            var airport = await GetAsync(items.Id);
            if (airport != null)
            {
                _context.Entry<Airport>(items).State = EntityState.Modified;
                _context.SaveChanges();
                _logger.LogInformation($"Airport updated with id {items.Id}");
                return airport;
            }
            throw new NoSuchAirportException();
        }
    }
}
