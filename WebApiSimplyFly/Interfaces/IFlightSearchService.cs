using WebApiSimplyFly.DTO;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Interfaces
{
    public interface IFlightSearchService
    {
        public Task<List<SearchedFlightResultDTO>> SearchFlights(SearchFlightDTO searchFlight);

        Task<List<int>> GetBookedSeatBySchedule(int scheduleID);

        public Task<List<Seat>> GetSeatDetailsByFlight(int FlightNo);

        public Task<bool> CheckSeatAvailablility(SeatCheckDTO seatCheckDTO);



    }
}
