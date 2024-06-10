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
    public class FlightSearchController : ControllerBase
    {
        private readonly IFlightSearchService _flightSearchService;
        private readonly ILogger<FlightSearchController> _logger;

        public FlightSearchController(IFlightSearchService flightSearchService, ILogger<FlightSearchController> logger)
        {
            _flightSearchService = flightSearchService;
            _logger = logger;
        }

        [HttpGet("GetAllFlightSeats")]
        public async Task<ActionResult<List<Seat>>> GetAllFlightSeatsAsync(int flightId)
        {
            var seats = await _flightSearchService.GetSeatDetailsByFlight(flightId);
            return Ok(seats);
        }

        [Route("GetBookedSeatsBiSchedule")]
        [HttpGet]
        public async Task<ActionResult<List<int>>> GetBookedSeatsBySchedule(int scheduleId)
        {
            try
            {
                var bookedSeats = await _flightSearchService.GetBookedSeatBySchedule(scheduleId);
                return bookedSeats;
            }
            catch (NoSuchBookingException nsbe)
            {
                _logger.LogInformation(nsbe.Message);
                return NotFound(nsbe.Message);
            }
        }

        [Route("SearchFlight")]
        [HttpGet]
        public async Task<ActionResult<List<SearchedFlightResultDTO>>> SearchFlight([FromQuery] SearchFlightDTO searchFlightDTO)
        {
            try
            {
                var flights = await _flightSearchService.SearchFlights(searchFlightDTO);
                return flights;
            }
            catch (NoFlightAvailableException nfae)
            {
                _logger.LogInformation(nfae.Message);
                return NotFound(nfae.Message);
            }

        }


        //not needed or not sure

        //[Route("CheckSeatAvailable")]
        //[HttpGet]
        //public async Task<ActionResult<bool>> CheckSeatAvailable([FromBody] SeatCheckDTO seatCheckDTO)
        //{
        //    try
        //    {
        //        var result = await _flightSearchService.CheckSeatAvailablility(seatCheckDTO);
        //        return result;
        //    }
        //    catch (NoSuchSeatException nfae)
        //    {
        //        _logger.LogInformation(nfae.Message);
        //        return NotFound(nfae.Message);
        //    }

        //}
    }
}
