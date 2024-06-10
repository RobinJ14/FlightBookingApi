using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiSimplyFly.DTO;
using WebApiSimplyFly.Exceptions;
using WebApiSimplyFly.Interfaces;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        private readonly IFlightService _flightService;
        private readonly ILogger<FlightController> _logger;
        public FlightController(IFlightService flightOwnerService,  ILogger<FlightController> logger)
        {
            _flightService = flightOwnerService;
            _logger = logger;
        }

        [HttpGet("GetAllFlight")]
        public async Task<ActionResult<List<Flight>>> GetAllFlight()
        {
            try
            {
                var flights = await _flightService.GetAllFlights();
                return flights;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpGet("GetFlightById/{flightId}")]
        public async Task<ActionResult<Flight>> GetFlightById(int flightId)
        {
            try
            {
                var flights = await _flightService.GetFlightById(flightId);
                return Ok(flights);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "flightOwner")]
        [HttpGet("GetFlightByOwner/{ownerId}")]
        public async Task<ActionResult<List<Flight>>> GetFlightByOwner(int ownerId)
        {
            try
            {
                var flights = await _flightService.GetFlightByFlightOwner(ownerId);
                return Ok(flights);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpPost("AddFlight")]
        public async Task<ActionResult<Flight>> AddFlight([FromBody]Flight flight)
        {
            try
            {
                flight = await _flightService.AddFlight(flight);
                return flight;
            }
            catch (FlightAlreadyPresentException fape)
            {
                _logger.LogInformation(fape.Message);
                return NotFound(fape.Message);
            }

        }

        [HttpDelete("RemoveFlight")]
        public async Task<ActionResult<Flight>> RemoveFlight(int flightNumber)
        {
            try
            {
                var flight = await _flightService.RemoveFlight(flightNumber);
                return flight;
            }
            catch (NoSuchFlightException nsfe)
            {
                _logger.LogInformation(nsfe.Message);
                return NotFound(nsfe.Message);
            }


        }

        [HttpPut("UpdateFlight")]
        public async Task<ActionResult<Flight>> UpdateFlight(int flightId, UpdateFlightDTO updateFlightDTO)
        {

            try
            {
                var flight = await _flightService.UpdateFlightDetails(flightId,updateFlightDTO);
                return Ok(flight);
            }
            catch (NoSuchFlightException nsfe)

            {
                _logger.LogInformation(nsfe.Message);
                return NotFound(nsfe.Message);
            }

        }
    }
}
