using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Interfaces
{
    public interface ISeatService
    {
        Task<Seat> AddSeatDetail(Seat flightOwner);
        Task<bool> RemoveSeatDetail(string id);
        Task<List<Seat>> GetAllSeatDetails();
        Task<Seat> GetByIdSeatDetails(string id);
    }
}
