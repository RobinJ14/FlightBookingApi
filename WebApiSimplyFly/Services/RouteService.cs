using WebApiSimplyFly.Exceptions;
using WebApiSimplyFly.Interfaces;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Services
{
    public class RouteService : IRouteService
    {
        private readonly IRepository<Models.Route,int> _routeRepository;
        private readonly ILogger<RouteService> _logger;
        private readonly IRepository<Airport,int> _airportRepository;
       
        public RouteService(IRepository<Models.Route,int> routeRepository, ILogger<RouteService> logger)
        {
            _routeRepository = routeRepository;
            _logger = logger;

        }
        public RouteService(IRepository< Models.Route,int> routeRepository, ILogger<RouteService> logger, IRepository<Airport,int> airportRepository)
        {
            _airportRepository = airportRepository;
            _routeRepository = routeRepository;
            _logger = logger;

        }
        

        public async Task<Models.Route> AddRoute(Models.Route route)
        {
            var existingRoutes = await GetAllRoutes();
            var existingRoute = existingRoutes.FirstOrDefault(s => s.SourceAirportId == route.SourceAirportId
             && s.DestinationAirportId == route.DestinationAirportId);

            if (existingRoute == null)
            {
                route = await _routeRepository.Add(route);
                return route;
            }
            throw new RouteAlreadyPresentException();
        }

        

        public async Task<List<Models.Route>> GetAllRoutes()
        {
            var routes = await _routeRepository.GetAsync();
            return routes;
        }

        public async Task<Models.Route> GetRouteById(int id)
        {
            var route = await _routeRepository.GetAsync(id);

            if (route != null)
            {
                return route;
            }
            throw new NoSuchRouteException();
        }

        public async Task<int> GetRouteIdByAirport(int sourceAirportId, int destinationAirportId)
        {
            var routes = await _routeRepository.GetAsync();
            var route = routes.FirstOrDefault(e => e.SourceAirportId == sourceAirportId &&
            e.DestinationAirportId == destinationAirportId);
            if (route != null)
            {
                return (int)route.RouteId;
            }
            throw new NoSuchRouteException();
        }

        public async Task<Models.Route> RemoveRoute(int sourceAirportId, int destinationAirportId)
        {
            var routes = await _routeRepository.GetAsync();
            var route = routes.FirstOrDefault(e => e.SourceAirportId == sourceAirportId
            && e.DestinationAirportId == destinationAirportId);

            if (route != null)
            {
                int routeId = route.RouteId;
                route = await _routeRepository.Delete(routeId);
                return route;
            }
            throw new NoSuchRouteException();
        }

        public async Task<bool> RemoveRouteById(int routeId)
        {
            if (await _routeRepository.Delete(routeId) != null)
            {
                return true;
            };
            return false;
        }

        public async Task<Models.Route> UpdateRoute(Models.Route route)
        {
            var updateRoute = await _routeRepository.GetAsync(route.RouteId);
            if (updateRoute != null)
            {
                updateRoute.RouteId=route.RouteId;
                updateRoute.SourceAirportId=route.SourceAirportId;
                updateRoute.DestinationAirportId = route.DestinationAirportId;

                updateRoute = await _routeRepository.Update(route);
                return updateRoute;
            }
            throw new NoSuchRouteException();
        }
    }
}
