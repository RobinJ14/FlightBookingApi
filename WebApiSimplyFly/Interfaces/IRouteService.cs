using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Interfaces
{
    public interface IRouteService
    {
        public Task<Airport> AddAirport(Airport airport);
        public Task<Models.Route> AddRoute(Models.Route route);
        public Task<Models.Route> RemoveRoute(int sourceAirportId, int destinationAirportId);
        public Task<List<Models.Route>> GetAllRoutes();
        public Task<Models.Route> GetRouteById(int id);
        public Task<int> GetRouteIdByAirport(int sourceAirportId, int destinationAirportId);
        public Task<List<Airport>> GetAllAirports();

        public Task<bool> RemoveRouteById(int routeId);
    }
}
