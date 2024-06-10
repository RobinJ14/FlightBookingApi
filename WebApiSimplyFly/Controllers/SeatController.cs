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
    public class SeatController : ControllerBase
    {
        private readonly ISeatService _seatService;
        private readonly ILogger<ScheduleController> _logger;

        public SeatController(ISeatService seatService, ILogger<ScheduleController> logger)
        {
            _seatService = seatService;
            _logger = logger;

        }


        [HttpGet("AllSeat")]
        public async Task<ActionResult<List<Seat>>> GetAllSeatDetail()
        {
            try
            {
                var seatDetails = _seatService.GetAllSeatDetails();
                return Ok(seatDetails);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return NotFound(ex.Message);
            }
        }


        [HttpGet("GetSeatById")]
        public async Task<ActionResult<Seat>> GetSeatDetailById(int id)
        {
            try
            {
                var seatDetail = _seatService.GetByIdSeatDetails(id);
                return Ok(seatDetail);
            }
            catch (NoSuchSeatException nsse)
            {
                _logger.LogInformation(nsse.Message);
                return NotFound(nsse.Message);
            }
        }


        [HttpPost("AddSeat")]
        public async Task<ActionResult<Seat>> AddSeatDetail(Seat seatDetail)
        {
            seatDetail = await _seatService.AddSeatDetail(seatDetail);
            if (seatDetail != null)
            {
                return Ok(seatDetail);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("AddBulkSeats")]
        public async Task<ActionResult<bool>> AddBulkSeatDetail([FromBody] UpdateSeatDTO seatDetail)
        {
            var result = await _seatService.AddSeatDetail2(seatDetail);
            if (result)
            {
                return Ok(result);

            }
            else
            {
                return NotFound();
            }
        }

        [Route("DeleteSeat")]
        [HttpDelete]
        public async Task<ActionResult<bool>> RemoveSeat(int seatId)
        {
            try
            {
                var result = await _seatService.RemoveSeatDetail(seatId);
                return result;
            }
            catch (NoSuchSeatException nsse)
            {
                _logger.LogInformation(nsse.Message);
                return NotFound(nsse.Message);
            }
        }

        [Route("DeleteSeatByFlight")]
        [HttpDelete]
        public async Task<ActionResult<bool>> RemoveSeatByFlight(int flightId)
        {
            try
            {
                var result = await _seatService.RemoveSeatByFlightDetail(flightId);
                return result;
            }
            catch (NoSuchSeatException nsse)
            {
                _logger.LogInformation(nsse.Message);
                return NotFound(nsse.Message);
            }
        }
    }
}
