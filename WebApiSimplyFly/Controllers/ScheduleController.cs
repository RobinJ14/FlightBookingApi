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
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;
        private readonly ILogger<ScheduleController> _logger;
        public ScheduleController(IScheduleService scheduleService, ILogger<ScheduleController> logger)
        {
            _scheduleService = scheduleService;
            _logger = logger;
        }

        [HttpGet("GetAllSchedule")]
        public async Task<ActionResult<List<Schedule>>> GetAllSchedule()
        {
            try
            {
                var schedules = await _scheduleService.GetAllSchedules();
                return schedules;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return NotFound(ex.Message);
            }


        }

        [Route("GetScheduleById/{id}")]
        [HttpGet]
        public async Task<ActionResult<Schedule>> GetScheduleById( int id)
        {
            try
            {
                var Schedule = await _scheduleService.GetScheduleById(id);
                return Schedule;
            }
            catch (NoSuchScheduleException nsse)

            {
                _logger.LogInformation(nsse.Message);
                return NotFound(nsse.Message);
            }
        }

        [Route("GetFlightSchedule")]
        [HttpGet]
        //[EnableCors("RequestPolicy")]
        public async Task<ActionResult<List<FlightScheduleDTO>>> GetFlightSchedule([FromQuery] int flightNumber)
        {
            try
            {
                var flightSchedule = await _scheduleService.GetFlightSchedules(flightNumber);
                return Ok(flightSchedule);
            }
            catch (NoSuchScheduleException nsse)

            {
                _logger.LogInformation(nsse.Message);
                return NotFound(nsse.Message);
            }
        }

        [HttpPost("AddSchedule")]
        public async Task<ActionResult<Schedule>> AddSchedule(Schedule schedule)
        {
            try
            {
                schedule = await _scheduleService.AddSchedule(schedule);
                return schedule;
            }
            catch (FlightScheduleBusyException fsbe)

            {
                _logger.LogInformation(fsbe.Message);
                return NotFound(fsbe.Message);
            }
        }

        [HttpDelete("DeleteScheduleById")]
        public async Task<ActionResult<bool>> DeleteSchedule([FromQuery] int Id)
        {
            try
            {
                var result = await _scheduleService.RemoveSchedule(Id);
                return Ok(result);
            }
            catch (NoSuchScheduleException nsse)
            {
                _logger.LogInformation(nsse.Message);
                return NotFound(nsse.Message);
            }
        }

        [Route("DeleteScheduleByFlight")]
        [HttpDelete]
        public async Task<ActionResult<int>> DeleteScheduleByFlight(int flightNumber)
        {
            try
            {
                var schedule = await _scheduleService.RemoveScheduleByFlight(flightNumber);
                return schedule;
            }
            catch (NoSuchScheduleException nsse)
            {
                _logger.LogInformation(nsse.Message);
                return NotFound(nsse.Message);
            }
        }

        [Route("DeleteScheduleByDate")]
        [HttpDelete]
        public async Task<ActionResult<int>> DeleteScheduleByDate([FromQuery] RemoveScheduleByDateDTO scheduleDTO)
        {
            try
            {
                var schedule = await _scheduleService.RemoveSchedule(scheduleDTO.DateOfSchedule, scheduleDTO.AirportId);
                return schedule;
            }
            catch (NoSuchScheduleException nsse)
            {
                _logger.LogInformation(nsse.Message);
                return NotFound(nsse.Message);
            }
        }

        [Route("UpdateScheduled")]
        [HttpPut]
        public async Task<ActionResult<Schedule>> UpdateScheduledTime(Schedule schedule)
        {
            try
            {
                var updateSchedule = await _scheduleService.UpdateSchedule(schedule);
                return schedule;
            }
            catch (NoSuchScheduleException nsse)
            {
                _logger.LogInformation(nsse.Message);
                return NotFound(nsse.Message);
            }
        }

    }
}
