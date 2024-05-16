using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Interfaces
{
    public interface IPassengerService
    {
        Task<Passenger> AddPassenger(Passenger flightOwner);
        Task<bool> RemovePassenger(int id);
        Task<List<Passenger>> GetAllPassengers();
        Task<Passenger> GetByIdPassengers(int id);
    }
}
