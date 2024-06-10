using WebApiSimplyFly.Interfaces;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Services
{
    public class PassengerService : IPassengerService
    {
        private readonly IRepository< Passenger,int> _passengerRepository;
        private readonly ILogger<PassengerService> _logger;

        public PassengerService(IRepository< Passenger,int> passengerRepository, ILogger<PassengerService> logger)
        {
            _passengerRepository = passengerRepository;
            _logger = logger;

        }
        public async Task<Passenger> AddPassenger(Passenger passenger)
        {
            return await _passengerRepository.Add(passenger);
        }

        public async Task<List<Passenger>> GetAllPassengers() 
        {
            return await _passengerRepository.GetAsync();
        }

        public async Task<Passenger> GetByIdPassengers(int id)
        {
            return await(_passengerRepository.GetAsync(id));
        }

        public async Task<bool> RemovePassenger(int id)
        {
            var owner = await _passengerRepository.GetAsync(id);
            if (owner != null)
            {
                await _passengerRepository.Delete(id);
                return true;
            }
            return false;
        }
    }
}
