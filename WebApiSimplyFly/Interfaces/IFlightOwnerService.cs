using WebApiSimplyFly.DTO;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Interfaces
{
    public interface IFlightOwnerService
    {
        public Task<FlightOwner> AddFlightOwner(FlightOwner flightowner);
        public Task<bool> RemoveFlightOwner(int Id);
        public Task<FlightOwner> GetFlightOwnerById(int id);
        public Task<List<FlightOwner>> GetAllFlightOwners();
        public Task<FlightOwner> GetByUsernameFlightOwners(string username);
        public Task<FlightOwner> UpdateFlightOwnerAddress(int id, string address);
        public Task<FlightOwner> UpdateFlightOwner(UpdateFlightOwnerDTO flightOwner);
    }
}
