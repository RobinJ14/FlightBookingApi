using WebApiSimplyFly.DTO;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Interfaces
{
    public interface ISeatService
    {
        Task<Seat> AddSeatDetail(Seat flightOwner);
        Task<bool> AddSeatDetail2(UpdateSeatDTO seatDetail);
        Task<bool> RemoveSeatDetail(int id);
        Task<bool> RemoveSeatByFlightDetail(int id);

        Task<List<Seat>> GetAllSeatDetails();
        Task<Seat> GetByIdSeatDetails(int id);
    }
}
