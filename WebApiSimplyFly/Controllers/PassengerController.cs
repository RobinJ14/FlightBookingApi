using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiSimplyFly.Interfaces;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassengerController : ControllerBase
    {
        private readonly IPassengerService _passengerService;
        private readonly ILogger<PassengerController> _logger;

        public PassengerController(IPassengerService passengerService,ILogger<PassengerController> logger)
        {
            _passengerService = passengerService;
            _logger = logger;

        }


        [HttpGet("GetAllPassenger")]
        public Task<List<Passenger>> GetAllPassenger()
        {
            var passengers = _passengerService.GetAllPassengers();
            return passengers;
        }

        [HttpGet("GetPassengerById")]
        public Task<Passenger> GetPassengerById(int id)
        {
            var passengers = _passengerService.GetByIdPassengers(id);
            return passengers;
        }

        [HttpPost("AddPassenger")]
        public async Task<Passenger> AddPassenger(Passenger passenger)
        {
            passenger = await _passengerService.AddPassenger(passenger);
            return passenger;
        }

        [HttpDelete("DeletePassenger")]
        public async Task<IActionResult> DeletePassenger(int passengerId)
        {
            var result = await _passengerService.RemovePassenger(passengerId);
            if (result)
            {
                return Ok(result);
            }
            return NotFound("User not found.");


        }
    }
}
