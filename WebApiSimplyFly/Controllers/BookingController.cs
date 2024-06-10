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
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly ILogger<BookingController> _logger;
        public BookingController(IBookingService bookingService, ILogger<BookingController> logger)
        {
            _bookingService = bookingService;
            _logger = logger;
        }

        [HttpPost("CreateBooking")]
        public async Task<IActionResult> CreateBooking([FromBody] BookingRequestDTO bookingRequestDto)
        {
            try
            {
                var bookingId = await _bookingService.CreateBookingAsync(bookingRequestDto);
                return Ok(bookingId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("CancelBooking/{bookingId}")]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            var success = await _bookingService.CancelBookingAsync(bookingId);
            if (success != null)
            {
                return NoContent();
            }
            return NotFound();
        }

        [HttpGet("GetAllbookings")]
        public async Task<ActionResult<List<Booking>>> GetAllBookings()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return Ok(bookings);
        }

        [HttpGet("{userId}/bookings/{bookingId}")]
        public async Task<IActionResult> GetBooking(int userId, int bookingId)
        {
            var booking = await _bookingService.GetBookingByIdAsync(bookingId);
            if (booking == null || booking.CustomerId != userId)
            {
                return NotFound();
            }
            return Ok(booking);
        }

        [Route("GetBookingByFlight")]
        [HttpGet]
        public async Task<ActionResult<List<Booking>>> GetBookingByFlight(int flightNumber)
        {
            try
            {
                var bookings = await _bookingService.GetBookingByFlight(flightNumber);
                return bookings;
            }
            catch (NoSuchBookingException nsbe)
            {
                _logger.LogInformation(nsbe.Message);
                return NotFound(nsbe.Message);
            }
        }

        [Route("GetBookingByOwnerId")]
        [HttpGet]
        public async Task<ActionResult<List<Booking>>> GetBookingByOwnerId(int ownerId)
        {
            try
            {
                var bookings = await _bookingService.GetBookingByOwnerId(ownerId);
                return bookings;
            }
            catch (NoSuchBookingException nsbe)
            {
                _logger.LogInformation(nsbe.Message);
                return NotFound(nsbe.Message);
            }
        }

        [Route("GetBookingBySchedule")]
        [HttpGet]
        public async Task<ActionResult<List<Booking>>> GetBookingBySchedule(int ScheduleId)
        {
            try
            {
                var bookings = await _bookingService.GetBookingBySchedule(ScheduleId);
                return bookings;
            }
            catch (NoSuchBookingException nsbe)
            {
                _logger.LogInformation(nsbe.Message);
                return NotFound(nsbe.Message);
            }
        }

        [HttpGet("GetBookingByCustomer/{CustomerId}")]
        public async Task<ActionResult<List<PassengerBooking>>> GetBookingByCustomer(int CustomerId)
        {
            try
            {
                var bookings = await _bookingService.GetBookingsByCustomerId(CustomerId);
                return bookings;
            }
            catch (NoSuchBookingException nsbe)
            {
                _logger.LogInformation(nsbe.Message);
                return NotFound(nsbe.Message);
            }
        }

        [HttpGet("GetBookingByCustomerBasic/{Id}")]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBasicBookingByCustomer(int Id)
        {
            try
            {
                var bookings = await _bookingService.GetBookingByCustomer(Id);
                return Ok(bookings);
            }
            catch (NoSuchBookingException nsbe)
            {
                _logger.LogInformation(nsbe.Message);
                return NotFound(nsbe.Message);
            }
        }


        [Route("GetPassengerBookingByBookingId")]
        [HttpGet]
        public async Task<ActionResult<List<PassengerBooking>>> GetPassengerBookingByBookingId(int bookingId)
        {
            try
            {
                var bookings = await _bookingService.GetPassengerBookingByBookingId(bookingId);
                return Ok(bookings);
            }
            catch (NoSuchBookingException nsbe)
            {
                _logger.LogInformation(nsbe.Message);
                return NotFound(nsbe.Message);
            }
        }



    }
}
