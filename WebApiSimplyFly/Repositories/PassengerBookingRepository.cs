using Microsoft.EntityFrameworkCore;
using WebApiSimplyFly.Context;
using WebApiSimplyFly.Exceptions;
using WebApiSimplyFly.Interfaces;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Repositories
{
    public class PassengerBookingRepository : IRepository<PassengerBooking, int>, IPassengerBookingRepository
    {
        private readonly newContext _context;
        private readonly ILogger<PassengerBookingRepository> _logger;
       
        public PassengerBookingRepository(newContext context, ILogger<PassengerBookingRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<PassengerBooking> Add(PassengerBooking items)
        {
            _context.Add(items);
            _context.SaveChanges();
            _logger.LogInformation($"PassengerBooking added with id {items.PassengerBookingId}");
            return items;
        }


        public async Task<PassengerBooking> Delete(int key)
        {
            var passengerBooking = await GetAsync(key);
            if (passengerBooking != null)
            {
                _context.Remove(passengerBooking);
                _context.SaveChanges();

                _logger.LogInformation($"PassengerBooking delete with id {key}");
                return passengerBooking;
            }
            throw new NoSuchPassengerBookingException();
        }

        public async Task<PassengerBooking> GetAsync(int key)
        {
            var passengerBookings = await GetAsync();
            var passengerBooking = passengerBookings.FirstOrDefault(e => e.PassengerBookingId == key);
            if (passengerBooking != null)
            {
                return passengerBooking;
            }
            throw new NoSuchPassengerBookingException();
        }

        public async Task<List<PassengerBooking>> GetAsync()
        {
            var passengerBookings = _context.PassengerBookings.Include(e => e.Booking).Include(e => e.Booking.Schedule)
              .Include(e => e.Passenger).Include(e => e.Seat).Include(e => e.Booking.Schedule.Route)
              .Include(e => e.Booking.Schedule.Flight).Include(e => e.Booking.Schedule.Route.SourceAirport)
              .Include(e => e.Booking.Schedule.Route.DestinationAirport)
              .ToList();
            return passengerBookings;
        }

        public async Task<List<string>> GetSeatNumbersForScheduleAsync(int scheduleId)
        {
            // Retrieve seat numbers for a specific schedule
            var seatNumbers = await _context.PassengerBookings
                .Where(pb => pb.Booking.ScheduleId == scheduleId)  // Filter by ScheduleId
                .Select(pb => pb.SeatNo)  // Select only the SeatNo property
                .ToListAsync();

            return seatNumbers;
        }



        public async Task<PassengerBooking> Update(PassengerBooking items)
        {
            var passengerBooking = await GetAsync(items.PassengerBookingId);
            if (passengerBooking != null)
            {
                _context.Entry<Models.PassengerBooking>(items).State = EntityState.Modified;
                _context.SaveChanges();
                return passengerBooking;
            }
            throw new NoSuchPassengerBookingException();
          
        }

        public async Task AddPassengerBookingAsync(PassengerBooking passengerBooking)
        {
            _context.PassengerBookings.Add(passengerBooking);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckSeatsAvailabilityAsync(int scheduleId, List<string> seatNos)
        {
            var bookedSeats = await _context.PassengerBookings
                            .Where(pb => pb.Booking.ScheduleId == scheduleId && seatNos.Contains(pb.SeatNo))
                            .Select(pb => pb.SeatNo)
                            .ToListAsync();

            // Check if any of the requested seats are already booked
            foreach (var seatNo in seatNos)
            {
                if (bookedSeats.Contains(seatNo))
                {
                    // Seat is already booked, return false
                    throw new Exception($"Seat {seatNo} is already booked.");

                }
            }

            // All seats are available
            return true;
        }


        public async Task<IEnumerable<PassengerBooking>> GetPassengerBookingsAsync(int bookingId)
        {
            return await _context.PassengerBookings
                .Where(pb => pb.BookingId == bookingId)
                .ToListAsync();
        }

        public async Task RemovePassengerBookingAsync(int passengerBookingId)
        {
            var passengerBooking = await _context.PassengerBookings.FindAsync(passengerBookingId);
            if (passengerBooking != null)
            {
                _context.PassengerBookings.Remove(passengerBooking);
                await _context.SaveChangesAsync();
            };
        }
    }
}
