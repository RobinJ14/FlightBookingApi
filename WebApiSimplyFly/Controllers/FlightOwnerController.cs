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
    public class FlightOwnerController : ControllerBase
    {
        private readonly IFlightOwnerService _flightOwnerService;
        private readonly ILogger<FlightOwnerController> _logger;

        public FlightOwnerController(IFlightOwnerService flightOwnerService, ILogger<FlightOwnerController> logger)
        {
            _flightOwnerService = flightOwnerService;
            _logger = logger;
        }

        [HttpGet("GetAllFlightOwners")]
        public async Task<ActionResult<List<FlightOwner>>> GetAllFlightOwnersAsync()
        {
            var users = await _flightOwnerService.GetAllFlightOwners();
            return Ok(users);
        }

        [HttpGet("FlightOwnerByUsername")]
        public async Task<ActionResult<FlightOwner>> GetFlightOwnerByUsername(string username)
        {
            try
            {
                var flightOwner = await _flightOwnerService.GetByUsernameFlightOwners(username);
                return Ok(flightOwner);
            }
            catch (NoSuchFlightOwnerException nsfe)
            {
                _logger.LogInformation(nsfe.Message);
                return NotFound(nsfe.Message);
            }
        }

        [HttpGet("GetFlightOwnerById/{id}")]
        public async Task<ActionResult<FlightOwner>> GetFlightOwnerById(int id)
        {
            try
            {
                var flightOwner = await _flightOwnerService.GetFlightOwnerById(id);
                return Ok(flightOwner);
            }
            catch (NoSuchFlightOwnerException nsfe)
            {
                _logger.LogInformation(nsfe.Message);
                return NotFound(nsfe.Message);
            }
        }


        [HttpPut("UpdateFlightOwner")]
        public async Task<ActionResult<FlightOwner>> UpdateFlightOwner(UpdateFlightOwnerDTO flightOwner)
        {
            try
            {
                var owner = await _flightOwnerService.UpdateFlightOwner(flightOwner);
                return Ok(owner);
            }
            catch (NoSuchFlightOwnerException nsfe)
            {
                _logger.LogInformation(nsfe.Message);
                return NotFound(nsfe.Message);
            }
        }

        [HttpDelete("DeleteflightOwners/{flightOwnerId}")]
        public async Task<IActionResult> DeleteFlightOwner(int flightOwnerId)
        {
            var result = await _flightOwnerService.RemoveFlightOwner(flightOwnerId);
            if (result)
            {
                return Ok(result);
            }
            return NotFound("User not found.");


        }
    }
}
