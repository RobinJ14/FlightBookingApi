using Microsoft.EntityFrameworkCore;
using WebApiSimplyFly.Context;
using WebApiSimplyFly.Exceptions;
using WebApiSimplyFly.Interfaces;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Repositories
{
    public class SeatRepository : IRepository<Seat, string>, ISeatRepository
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
            _logger.LogInformation("Seat detail added with seatDetailId" + items.SeatNo);
            return items;
        }

        public async Task<Seat> Delete(string key)
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

        public async Task<Seat> GetAsync(string key)
        {
            var seatDetails = await GetAsync();
            var seatDetail = seatDetails.FirstOrDefault(e => e.SeatNo == key);
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
            var seatDetail = await GetAsync(items.SeatNo);
            if (seatDetail != null)
            {
                _context.Entry(items).State = EntityState.Modified;
                _context.SaveChanges();
                _logger.LogInformation("Seat detail updated");
                return seatDetail;
            }
            throw new NoSuchSeatException();
        }
        public async Task<IEnumerable<Seat>> GetSeatDetailsAsync(List<string> seatNos)
        {
            return await Task.FromResult(_context.Seats.Where(s => seatNos.Contains(s.SeatNo)).ToList());
        }

        public async Task UpdateSeatDetailsAsync(IEnumerable<Seat> seats)
        {
            _context.Seats.UpdateRange(seats);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Seat detail updated");
        }

        public async Task<List<Seat>> GetSeatDetailsByFlight(string FlightNo)
        {
            var seatDetails = await GetAsync();
            var seatsForFlight = seatDetails.Where(e => e.FlightNo == FlightNo).ToList();

            if (seatsForFlight.Any())
            {
                return seatsForFlight;
            }

            throw new NoSuchSeatException();
        }
    }
}
