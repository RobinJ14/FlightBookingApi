using Microsoft.EntityFrameworkCore;
using WebApiSimplyFly.Context;
using WebApiSimplyFly.Exceptions;
using WebApiSimplyFly.Interfaces;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Repositories
{
    public class FlightOwnerRepository : IRepository<FlightOwner, int>
    {
        private readonly newContext _context;
        private readonly ILogger<FlightOwnerRepository> _logger;

        /// <summary>
        /// Default constructor with RequestTrackerContext
        /// </summary>
        /// <param name="context">Database context</param>
        public FlightOwnerRepository(newContext context, ILogger<FlightOwnerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<FlightOwner> Add(FlightOwner item)
        {
            _context.Add(item);
            _context.SaveChanges();
            _logger.LogInformation("FlightOwner added " + item.OwnerId);
            return item;
        }

        public async Task<FlightOwner> Delete(int key)
        {
            var owner = await GetAsync(key);
            if (owner != null)
            {
                _context?.Remove(owner);
                _context.SaveChanges();
                _logger.LogInformation("FlightOwner deleted with id" + key);
                return owner;
            }
            throw new NoSuchFlightOwnerException();
        }

        public async Task<FlightOwner> GetAsync(int key)
        {
            var flightOwners = await GetAsync();
            var flightOwner = flightOwners.FirstOrDefault(e => e.OwnerId == key);
            if (flightOwner != null)
            {
                return flightOwner;
            }
            throw new NoSuchFlightOwnerException();
        }

        public async Task<List<FlightOwner>> GetAsync()
        {
            var flightOwners = _context.FlightsOwners.Include(e => e.OwnedFlights).ToList();
            return flightOwners;
        }

        public async Task<FlightOwner> Update(FlightOwner items)
        {
            var flightOwner = await GetAsync(items.OwnerId);

            _context.Entry<FlightOwner>(items).State = EntityState.Modified;
            _context.SaveChanges();
            _logger.LogInformation("FlightOwner updated with id" + items.OwnerId);
            return flightOwner;

        }
    }
}
