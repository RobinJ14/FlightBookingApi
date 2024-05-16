using WebApiSimplyFly.Interfaces;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Services
{
    public class SeatService : ISeatService
    {
        private readonly IRepository<Seat, string> _seatRepository;
        private readonly ILogger<SeatService> _logger;

        public SeatService(IRepository<Seat, string> seatRepository, ILogger<SeatService> logger)
        {
            _seatRepository = seatRepository;
            _logger = logger;

        }
        public async Task<Seat> AddSeatDetail(Seat seatDetail)
        {
            return await _seatRepository.Add(seatDetail);
        }

        public async Task<List<Seat>> GetAllSeatDetails()
        {
            return await _seatRepository.GetAsync();
        }

        public async Task<Seat> GetByIdSeatDetails(string id)
        {
            return await(_seatRepository.GetAsync(id));
        }

        public async Task<bool> RemoveSeatDetail(string id)
        {
            var owner = await _seatRepository.GetAsync(id);
            if (owner != null)
            {
                await _seatRepository.Delete(id);
                return true;
            }
            return false;
        }
    }
}




