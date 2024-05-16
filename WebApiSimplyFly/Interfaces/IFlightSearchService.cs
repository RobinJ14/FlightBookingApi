using WebApiSimplyFly.DTO;

namespace WebApiSimplyFly.Interfaces
{
    public interface IFlightSearchService
    {
        public Task<List<SearchedFlightResultDTO>> SearchFlights(SearchFlightDTO searchFlight);

    }
}
