using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiSimplyFly.DTO;
using WebApiSimplyFly.Exceptions;
using WebApiSimplyFly.Interfaces;

namespace WebApiSimplyFly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouteController : ControllerBase
    {
        private readonly IRouteService _routeService;
        private readonly ILogger<RouteController> _logger;
        public RouteController(IRouteService routeService, ILogger<RouteController> logger)
        {
            _routeService = routeService;
            _logger = logger;

        }

        [HttpGet("GetAllRoute")]
        public async Task<ActionResult<List<Models.Route>>> GetAllRoute()
        {
            try
            {
                var routes = await _routeService.GetAllRoutes();
                return routes;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return NotFound(ex.Message);
            }

        }

        [Route("AddRoute")]
        [HttpPost]
        public async Task<ActionResult<Models.Route>> AddRoute(Models.Route route)
        {
            try
            {
                route = await _routeService.AddRoute(route);
                return route;
            }
            catch (RouteAlreadyPresentException nape)
            {
                _logger.LogInformation(nape.Message);
                return NotFound(nape.Message);
            }


        }

        [HttpDelete("RemoveRouteByAirport")]
        public async Task<ActionResult<Models.Route>> RemoveRouteByAirport([FromQuery] RouteDTO routeDTO)
        {
            try
            {
                var route = await _routeService.RemoveRoute(routeDTO.SourceAirportId, routeDTO.DestinationAirportId);
                return route;
            }
            catch (NoSuchRouteException nsre)
            {
                _logger.LogInformation(nsre.Message);
                return NotFound(nsre.Message);
            }


        }

        [Route("GetRouteIdByAirport")]
        [HttpGet]
        public async Task<ActionResult<int>> GetRouteIdByAirport([FromQuery] RouteDTO getRouteIdDTO)
        {
            try
            {
                int routeId = await _routeService.GetRouteIdByAirport(getRouteIdDTO.SourceAirportId, getRouteIdDTO.DestinationAirportId);
                return routeId;
            }
            catch (NoSuchRouteException nsre)
            {
                _logger.LogInformation(nsre.Message);
                return NotFound(nsre.Message);
            }
        }

        [HttpDelete("RemoveRouteById/{id}")]
        public async Task<ActionResult<bool>> RemoveRouteById(int id)
        {
            try
            {
                var result = await _routeService.RemoveRouteById(id);
                return result;
            }
            catch (NoSuchRouteException nsre)
            {
                _logger.LogInformation(nsre.Message);
                return NotFound(nsre.Message);
            }


        }

        [Route("GetRouteById/{id}")]
        [HttpGet]
        public async Task<ActionResult<Models.Route>> GetRouteId(int id)
        {
            try
            {
                var route = await _routeService.GetRouteById(id);
                return route;
            }
            catch (NoSuchRouteException nsre)
            {
                _logger.LogInformation(nsre.Message);
                return NotFound(nsre.Message);
            }
        }

        [HttpPut("UpdateRoute")]
        public async Task<ActionResult<Models.Route>> UpdateRoute(Models.Route route)
        {
            try
            {
                var route1 = await _routeService.UpdateRoute(route);
                return Ok(route1);
            }
            catch (NoSuchRouteException nsfe)
            {
                _logger.LogInformation(nsfe.Message);
                return NotFound(nsfe.Message);
            }
        }
    }
}
