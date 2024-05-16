using Microsoft.EntityFrameworkCore;
using WebApiSimplyFly.Context;
using WebApiSimplyFly.Exceptions;
using WebApiSimplyFly.Interfaces;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Repositories
{
    public class BookingRepository : IRepository<Booking, int>, IBookingRepository
    {
        private readonly newContext _context;
        private readonly ILogger<BookingRepository> _logger;
        
        public BookingRepository(newContext context, ILogger<BookingRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Booking> Add(Booking items)
        {
            _context.Add(items);
            _context.SaveChanges();
            _logger.LogInformation($"Booking added with id {items.BookingId}");
            return items;
        }

        public async Task<Booking> Delete(int key)
        {
            var booking = await GetAsync(key);
            if (booking != null)
            {
                _context.Remove(booking);
                _context.SaveChanges();
                _logger.LogInformation($"Booking deleted with id {key}");
                return booking;
            }
            throw new NoSuchBookingException();
        }

        public async Task<Booking> GetAsync(int key)
        {
            var bookings = await GetAsync();
            var booking = bookings.FirstOrDefault(e => e.BookingId == key);
            if (booking != null)
            {
                return booking;
            }
            throw new NoSuchBookingException();
        }

        public async Task<List<Booking>> GetAsync()
        {
            var bookings = _context.Bookings.Include(e => e.Schedule).Include(e => e.Payment)
                .Include(e => e.Schedule.Route).Include(e => e.Schedule.Flight)
                .Include(e => e.Schedule.Route.SourceAirport).Include(e => e.Schedule.Route.DestinationAirport)
                .ToList();
            return bookings;
        }

        public async Task<Booking> Update(Booking items)
        {
            var bookings = await GetAsync(items.BookingId);
            if (bookings != null)
            {
                _context.Entry<Booking>(items).State = EntityState.Modified;
                _context.SaveChanges();
                _logger.LogInformation($"Booking updated with id {items.BookingId}");
                return bookings;
            }
            throw new NoSuchBookingException();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByCustomerIdAsync(int CustomerId)
        {
            return await _context.Bookings
                .Where(b => b.CustomerId == CustomerId)
                .ToListAsync();
        }
    }
}
