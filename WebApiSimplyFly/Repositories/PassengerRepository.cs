using Microsoft.EntityFrameworkCore;
using WebApiSimplyFly.Context;
using WebApiSimplyFly.Exceptions;
using WebApiSimplyFly.Interfaces;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Repositories
{
    public class PassengerRepository : IRepository<Passenger, int>
    {
        private readonly newContext _context;
        private readonly ILogger<PassengerRepository> _logger;

        
        public PassengerRepository(newContext context, ILogger<PassengerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Passenger> Add(Passenger items)
        {
            _context.Add(items);
            _context.SaveChanges();
            _logger.LogInformation($"Passenger added with id {items.PassengerId}");
            return items;
        }

        public async Task<Passenger> Delete(int key)
        {
            var passenger = await GetAsync(key);
            if (passenger != null)
            {
                _context.Remove(passenger);
                _context.SaveChanges();
                _logger.LogInformation($"Passenger deleted with id {key}");
                return passenger;
            }
            throw new NoSuchPassengerException();
        }

        public async Task<Passenger> GetAsync(int key)
        {
            var passengers = await GetAsync();
            var passenger = passengers.FirstOrDefault(e => e.PassengerId == key);
            if (passenger != null)
            {
                return passenger;
            }
            throw new NoSuchPassengerException();
        }

        public async Task<List<Passenger>> GetAsync()
        {
            var passengers = _context.Passengers.ToList();
            return passengers;
        }

        public async Task<Passenger> Update(Passenger items)
        {
            var passenger = await GetAsync(items.PassengerId);
            if (passenger != null)
            {
                _context.Entry<Models.Passenger>(items).State = EntityState.Modified;
                _context.SaveChanges();
                _logger.LogInformation($"Passenger updated with id {items.PassengerId}");
                return passenger;
            }
            throw new NoSuchPassengerException();
        }
    }
}
