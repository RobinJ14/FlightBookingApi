using Microsoft.EntityFrameworkCore;
using WebApiSimplyFly.Context;
using WebApiSimplyFly.Exceptions;
using WebApiSimplyFly.Interfaces;

namespace WebApiSimplyFly.Repositories
{
    public class RouteRepository : IRepository<Models.Route, int>
    {
        private readonly newContext _context;
        private readonly ILogger<RouteRepository> _logger;

  
        public RouteRepository(newContext context, ILogger<RouteRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Models.Route> Add(Models.Route items)
        {
            _context.Add(items);
            _context.SaveChanges();
            _logger.LogInformation("Route added with routeId" + items.RouteId);
            return items;
        }

        public async Task<Models.Route> Delete(int key)
        {
            var route = await GetAsync(key);
            if (route != null)
            {
                _context.Remove(route);
                _context.SaveChanges();
                _logger.LogInformation("Route deleted with routeId" + key);
                return route;
            }
            throw new NoSuchRouteException();
        }

        public async Task<Models.Route> GetAsync(int key)
        {
            var routes = await GetAsync();
            var route = routes.FirstOrDefault(e => e.RouteId == key);
            if (route != null)
            {
                return route;
            }
            throw new NoSuchRouteException();
        }

        public async Task<List<Models.Route>> GetAsync()
        {
            var routes = _context.Routes.Include(e => e.SourceAirport).Include(d => d.DestinationAirport).ToList();
            return routes;
        }

        public async Task<Models.Route> Update(Models.Route items)
        {
            var route = await GetAsync(items.RouteId);
            if (route != null)
            {
                _context.Entry<Models.Route>(items).State = EntityState.Modified;
                _context.SaveChanges();
                _logger.LogInformation("Route updated with routeId" + items.RouteId);
                return route;
            }
            throw new NoSuchRouteException();
        }
    }
}
