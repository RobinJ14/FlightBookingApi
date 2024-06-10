using Microsoft.EntityFrameworkCore;
using WebApiSimplyFly.Context;
using WebApiSimplyFly.Exceptions;
using WebApiSimplyFly.Interfaces;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Repositories
{
    public class SeatRepository : IRepository<Seat, int>, ISeatRepository
    {
        newContext _context;
        ILogger<SeatRepository> _logger;

        public SeatRepository(newContext context, ILogger<SeatRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Seat> Add(Seat items)
        {
            _context.Add(items);
            _context.SaveChanges();
            _logger.LogInformation("Seat detail added with seatDetailId" + items.SeatId);
            return items;
        }

        public async Task<Seat> Delete(int key)
        {
            var seatDetail = await GetAsync(key);
            if (seatDetail != null)
            {
                _context.Remove(seatDetail);
                _context.SaveChanges();
                _logger.LogInformation("Seat detail deleted with seatDetailId" + key);
                return seatDetail;
            }
            throw new NoSuchSeatException();
        }

        public async Task<Seat> GetAsync(int key)
        {
            var seatDetails = await GetAsync();
            var seatDetail = seatDetails.FirstOrDefault(e => e.SeatId == key);
            if (seatDetail != null)
            {
                return seatDetail;
            }
            throw new NoSuchSeatException();
        }

        public async Task<List<Seat>> GetAsync()
        {
            var seatDetails = _context.Seats.ToList();
            return seatDetails;
        }

      
        public async Task<Seat> Update(Seat items)
        {
            var seatDetail = await GetAsync(items.SeatId);
            if (seatDetail != null)
            {
                _context.Entry(items).State = EntityState.Modified;
                _context.SaveChanges();
                _logger.LogInformation("Seat detail updated");
                return seatDetail;
            }
            throw new NoSuchSeatException();
        }



        public async Task<IEnumerable<Seat>> GetSeatDetailsAsync(List<int> seatNos)
        {
            return await Task.FromResult(_context.Seats.Where(s => seatNos.Contains(s.SeatId)).ToList());
        }

        public async Task<bool> AddSeatDetailsAsync(IEnumerable<Seat> seats)
        {
            if (seats.Count()>0)
            {
                _context.Seats.AddRange(seats);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Seat detail updated");
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<Seat>> GetSeatDetailsByFlight(int FlightNo)
        {
            var seatDetails = await GetAsync();
            var seatsForFlight = seatDetails.Where(e => e.FlightId == FlightNo).ToList();

            if (seatsForFlight.Any())
            {
                return seatsForFlight;
            }

            throw new NoSuchSeatException();
        }

        public async Task<bool> DeleteSeatByFlightId(int flightId)
        {
            var seatDetails = await _context.Seats.Where(seat => seat.FlightId == flightId).ToListAsync();

            if (seatDetails != null && seatDetails.Count > 0)
            {
                _context.Seats.RemoveRange(seatDetails);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Seat details deleted with FlightId: {flightId}");
                return true;
            }

            throw new NoSuchSeatException();
        }

    }
}
