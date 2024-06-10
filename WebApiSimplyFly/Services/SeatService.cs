using WebApiSimplyFly.DTO;
using WebApiSimplyFly.Interfaces;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Services
{
    public class SeatService : ISeatService
    {
        private readonly IRepository<Seat, int> _seatRepository;
        private readonly ISeatRepository _seatRepository2;

        private readonly ILogger<SeatService> _logger;

        public SeatService(IRepository<Seat, int> seatRepository, ISeatRepository seatRepository1, ILogger<SeatService> logger)
        {
            _seatRepository = seatRepository;
            _seatRepository2 = seatRepository1;
            _logger = logger;

        }
        public async Task<Seat> AddSeatDetail(Seat seatDetail)
        {
            return await _seatRepository.Add(seatDetail);
        }

        public async Task<bool> AddSeatDetail2(UpdateSeatDTO seatDetail)
        {
            var seats = new List<Seat>();

            if (seatDetail.EconomyCount>0)
            {
                AddSeat(seatDetail.FlightId, "Economy", seatDetail.EconomyCount,seats);

            }

            if (seatDetail.PremiumCount > 0)
            {
                AddSeat(seatDetail.FlightId, "Premium", seatDetail.PremiumCount,seats);

            }

            if (seatDetail.PremiumEconomyCount > 0)
            {
                AddSeat(seatDetail.FlightId, "premiumEconomy", seatDetail.PremiumEconomyCount, seats);

            }

            var result = await _seatRepository2.AddSeatDetailsAsync(seats);
            return result;


        }

        public async Task<List<Seat>> GetAllSeatDetails()
        {
            return await _seatRepository.GetAsync();
        }

        public async Task<Seat> GetByIdSeatDetails(int id)
        {
            return await(_seatRepository.GetAsync(id));
        }

        public async Task<bool> RemoveSeatDetail(int id)
        {
            var owner = await _seatRepository.GetAsync(id);
            if (owner != null)
            {
                await _seatRepository.Delete(id);
                return true;
            }
            return false;
        }

        public async Task<bool> RemoveSeatByFlightDetail(int id)
        {
            var owner = await _seatRepository2.GetSeatDetailsByFlight(id);
            if (owner != null)
            {
                await _seatRepository2.DeleteSeatByFlightId(id);
                return true;
            }
            return false;
        }


        public void AddSeat(int flightId, string seatClass, int seatCount, List<Seat> seats)
        {

            for (int i = 1; i <= seatCount; i++)
            {
                seats.Add(new Seat
                {
                    SeatClass = seatClass,
                    FlightId = flightId
                });
            }
        }
    }
}




