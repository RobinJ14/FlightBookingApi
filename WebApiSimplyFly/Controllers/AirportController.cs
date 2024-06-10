using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiSimplyFly.Exceptions;
using WebApiSimplyFly.Interfaces;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirportController : ControllerBase
    {
        private readonly IAirportService _airportService;
        private readonly ILogger<AirportController> _logger;

        public AirportController(IAirportService airportService, ILogger<AirportController> logger)
        {
            _airportService = airportService;
            _logger = logger;
        }

        [Route("GetAllAirport")]
        [HttpGet]
        public async Task<ActionResult<List<Airport>>> GetAllAirport()
        {
            try
            {
                var airport = await _airportService.GetAllAirports();
                return Ok(airport);
            }
            catch (NoSuchAirportException nsae)
            {
                _logger.LogInformation(nsae.Message);
                return NotFound(nsae.Message);
            }
        }

        [HttpGet("GetAirportById")]
        public async Task<ActionResult<Airport>> GetAirportById(int Id)
        {
            try
            {
                var airport = await _airportService.GetByIdAirport(Id);
                return Ok(airport);
            }
            catch (NoSuchAirportException nsae)
            {
                _logger.LogInformation(nsae.Message);
                return NotFound(nsae.Message);
            }
        }


        [HttpPut("UpdateAirport")]
        public async Task<ActionResult<Airport>> UpdateAirport(Airport airport)
        {
            try
            {
                var updatedAirport = await _airportService.UpdateAirport(airport);
                return Ok(updatedAirport);
            }
            catch (NoSuchAirportException nsae)
            {
                _logger.LogInformation(nsae.Message);
                return NotFound(nsae.Message);
            }
        }

        [Route("DeleteAirport")]
        [HttpDelete]
        public async Task<ActionResult<bool>> RemoveAirport(int Id)
        {
            try
            {
                var airport = await _airportService.RemoveAirport(Id);
                return Ok(airport);
            }
            catch (NoSuchAirportException nsae)
            {
                _logger.LogInformation(nsae.Message);
                return NotFound(nsae.Message);
            }
        }

        [HttpPost("Addairport")]
        public async Task<ActionResult<Airport>> AddAirport(Airport airport)
        {
            try
            {
                airport = await _airportService.AddAirport(airport);
                return airport;
            }
            catch (AirportAlreadyPresentException aape)
            {
                _logger.LogInformation(aape.Message);
                return NotFound(aape.Message);
            }
        }


    }
}

