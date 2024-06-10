using WebApiSimplyFly.DTO;
using WebApiSimplyFly.Exceptions;
using WebApiSimplyFly.Interfaces;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Services
{
    public class FlightOwnerService : IFlightOwnerService
    {
        private readonly IRepository<User,string> _userRepository;
        private readonly IRepository<FlightOwner,int> _flightownerRepository;
        private readonly ILogger<FlightOwnerService> _logger;
        public FlightOwnerService(IRepository<FlightOwner,int> flightownerRepository, IRepository<User, string> userRepository, ILogger<FlightOwnerService> logger)
        {
            _userRepository = userRepository;
            _flightownerRepository = flightownerRepository;
            _logger = logger;

        }
        public async Task<FlightOwner> AddFlightOwner(FlightOwner flightOwner)
        {
            return await _flightownerRepository.Add(flightOwner);
        }

        public async Task<List<FlightOwner>> GetAllFlightOwners()
        {
            return await _flightownerRepository.GetAsync();
        }

        public async Task<FlightOwner> GetByUsernameFlightOwners(string username)
        {
            var flightOwners = await _flightownerRepository.GetAsync();
            var flightOwner = flightOwners.FirstOrDefault(e => e.Username == username);
            if (flightOwner != null)
            {
                return flightOwner;
            }
            throw new NoSuchFlightOwnerException();
        }

        public async Task<FlightOwner> GetFlightOwnerById(int id)
        {
            var flightOwners = await _flightownerRepository.GetAsync();
            var flightOwner = flightOwners.FirstOrDefault(e => e.OwnerId == id);
            if (flightOwner != null)
            {
                return flightOwner;
            }
            throw new NoSuchFlightOwnerException();
        }

        public async Task<bool> RemoveFlightOwner(int id)
        {
            var owner = await _flightownerRepository.GetAsync(id);
            if (owner != null)
            {
                await _flightownerRepository.Delete(id);
                await _userRepository.Delete(owner.Username);

                return true;
            }
            return false;
        }

        public async Task<FlightOwner> UpdateFlightOwner(UpdateFlightOwnerDTO flightOwner)
        {
            var owner = await _flightownerRepository.GetAsync(flightOwner.OwnerId);
            if (owner != null)
            {
                owner.Name = flightOwner.Name;
                owner.Email = flightOwner.Email;
                owner.Phone = flightOwner.Phone;
                owner.CompanyName = flightOwner.CompanyName;
                owner.Address = flightOwner.Address;
                owner = await _flightownerRepository.Update(owner);
                return owner;
            }
            throw new NoSuchFlightOwnerException();
        }



        
    }
}
