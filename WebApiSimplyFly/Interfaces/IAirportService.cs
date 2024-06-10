using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Interfaces
{
    public interface IAirportService
    {
        public Task<Airport> AddAirport(Airport airport);
        public Task<List<Airport>> GetAllAirports();
        public Task<bool> RemoveAirport(int Id);
        public Task<Airport> GetByIdAirport(int id);
        public Task<Airport> UpdateAirport(Airport airport);
    }
}
