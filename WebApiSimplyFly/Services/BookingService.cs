using WebApiSimplyFly.DTO;
using WebApiSimplyFly.Exceptions;
using WebApiSimplyFly.Interfaces;
using WebApiSimplyFly.Models;
using static WebApiSimplyFly.Models.Payment;

namespace WebApiSimplyFly.Services
{
    public class BookingService : IBookingService
    {
        private readonly IRepository<Booking,int> _bookingRepository;
        private readonly IRepository<Flight,string> _flightRepository;
        private readonly IRepository<PassengerBooking,int> _passengerBookingRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly IRepository<Schedule,int> _scheduleRepository;
        private readonly IPassengerBookingRepository _passengerBookingsRepository;
        private readonly IBookingRepository _bookingsRepository;
        private readonly IRepository<Payment,int> _paymentRepository;
        private readonly ILogger<BookingService> _logger;

        public BookingService(IRepository<Booking,int> bookingRepository, IRepository<Schedule,int> scheduleRepository, IRepository<PassengerBooking,int> passengerBookingRepository, IRepository<Flight,string> flightRepository, IBookingRepository bookingsRepository, ISeatRepository seatRepository, IPassengerBookingRepository passengerBookingRepository1, IRepository<Payment,int> paymentRepository, ILogger<BookingService> logger)
        {
            _flightRepository = flightRepository;
            _bookingsRepository = bookingsRepository;
            _passengerBookingsRepository = passengerBookingRepository1;
            _bookingRepository = bookingRepository;
            _passengerBookingRepository = passengerBookingRepository;
            _seatRepository = seatRepository;
            _scheduleRepository = scheduleRepository;
            _paymentRepository = paymentRepository;
            _logger = logger;
        }

        public async Task<bool> CreateBookingAsync(BookingRequestDTO bookingRequest)
        {
            if (bookingRequest == null)
            {
                throw new ArgumentNullException(nameof(bookingRequest));
            }

            var isSeatsAvailable = await _passengerBookingsRepository.CheckSeatsAvailabilityAsync(bookingRequest.ScheduleId, bookingRequest.SelectedSeats);
            if (!isSeatsAvailable)
            {
                // Handle case where selected seats are not available
                return false;
            }

            var schedule = await _scheduleRepository.GetAsync(bookingRequest.ScheduleId);
            if (schedule == null)
            {
                throw new Exception("Schedule not found.");
            }

            // Calculate total price based on the number of selected seats
            var totalPrice = CalculateTotalPrice(bookingRequest.SelectedSeats.Count, await _flightRepository.GetAsync(schedule.FlightNo));
            var seatClass = bookingRequest.SelectedSeats[0][0];
            if (seatClass == 'E')
            {
                totalPrice += 800;
            }

            // Create Payment entry
            var payment = new Payment
            {
                Amount = bookingRequest.Price,
                PaymentDate = DateTime.Now,
                Status = PaymentStatus.Successful,
                PaymentDetails = bookingRequest.PaymentDetails,
            };
            var addedPayment = await _paymentRepository.Add(payment);

            // Create Booking object
            var booking = new Booking
            {
                ScheduleId = bookingRequest.ScheduleId,
                CustomerId = bookingRequest.UserId,
                BookingTime = DateTime.Now, // Set current booking time
                TotalPrice = bookingRequest.Price,
                PaymentId = addedPayment.PaymentId // Assign the PaymentId of the created payment
            };


            // Save booking
            var addedBooking = await _bookingRepository.Add(booking);
            // Fetch seat details for selected seats
            var seatDetails = await _seatRepository.GetSeatDetailsAsync(bookingRequest.SelectedSeats);

            if (seatDetails == null || seatDetails.Count() != bookingRequest.SelectedSeats.Count())
            {
                throw new Exception("Invalid seat selection.");
            }


            // Create PassengerBooking entries,SeatNumbers
            int index = 0;
            foreach (var passengerId in bookingRequest.PassengerIds)
            {
                var seatDetail = seatDetails.ElementAtOrDefault(index); // Get the seat detail at the current index
                if (seatDetail != null)
                {
                    var passengerBooking = new PassengerBooking
                    {
                        BookingId = addedBooking.BookingId,
                        PassengerId = passengerId,
                        SeatNo = seatDetail.SeatNo // Assign a unique seat to each passenger
                    };
                    await _passengerBookingRepository.Add(passengerBooking);
                    index++; // Move to the next seat for the next passenger
                }
                else
                {
                    // Handle case where there are not enough seats for all passengers
                    throw new Exception("Not enough seats available for all passengers.");
                }
            }

            return addedBooking != null && addedPayment != null;
        }

        public async Task<Booking> CancelBookingAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetAsync(bookingId);

            if (booking == null)
            {
                throw new NoSuchBookingException();
            }


            // Remove passenger bookings also passengers is to be done
            var passengerBookings = await _passengerBookingsRepository.GetPassengerBookingsAsync(bookingId);
            foreach (var passengerBooking in passengerBookings)
            {
                await _passengerBookingRepository.Delete(passengerBooking.PassengerBookingId);
            }

            // Delete payment instead change status
            //await _paymentRepository.Delete(booking.PaymentId);

            // Delete booking instaed change status
            return await _bookingRepository.Delete(booking.BookingId);
        }

        public async Task<PassengerBooking> CancelBookingByPassenger(int passengerId)
        {
            var passengerBooking = await _passengerBookingRepository.GetAsync(passengerId);
            if (passengerBooking != null)
            {
                passengerBooking = await _passengerBookingRepository.Delete(passengerId);
                return passengerBooking;
            }
            throw new NoSuchPassengerException();
        }

       

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _bookingRepository.GetAsync();
        }

        public async Task<List<string>> GetBookedSeatBySchedule(int scheduleID)
        {
            var bookings = await _passengerBookingRepository.GetAsync();
            var bookedSeats = bookings.Where(e => e.Booking.ScheduleId == scheduleID)
                .Select(e => e.SeatNo).ToList();
            if (bookedSeats != null)
            {
                return bookedSeats;
            }
            throw new NoSuchBookingException();
        }

        public async Task<List<Booking>> GetBookingByFlight(string flightNumber)
        {
            var bookings = await _bookingRepository.GetAsync();
            bookings = bookings.Where(e => e.Schedule.FlightNo == flightNumber).ToList();
            if (bookings != null)
            {
                return bookings;
            }
            throw new NoSuchBookingException();
        }

        public async Task<Booking> GetBookingByIdAsync(int bookingId)
        {
            return await _bookingRepository.GetAsync(bookingId);
        }

        public async Task<List<Booking>> GetBookingBySchedule(int scheduleId)
        {
            var bookings = await _bookingRepository.GetAsync();
            bookings = bookings.Where(e => e.ScheduleId == scheduleId).ToList();


            if (bookings != null)
            {
                return bookings;
            }
            throw new NoSuchBookingException();
        }

        public async Task<List<PassengerBooking>> GetBookingsByCustomerId(int customerId)
        {
            var bookings = await _passengerBookingRepository.GetAsync();
            bookings = bookings.Where(e => e.Booking.CustomerId == customerId).ToList();
            if (bookings != null)
            {
                return bookings;
            }
            throw new NoSuchCustomerException();
        }

        public async Task<IEnumerable<Booking>> GetUserBookingsAsync(int userId)
        {
            var bookings = await _bookingsRepository.GetBookingsByCustomerIdAsync(userId);
            if (bookings != null)
            {
               bookings= bookings.Where(e => e.Schedule.DepartureTime < DateTime.Now);
            }
            return bookings;
        }

        public async Task<bool> RequestRefundAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetAsync(bookingId);

            if (booking == null)
            {
                throw new NoSuchBookingException();
            }

            // Check if payment exists
            var payment = await _paymentRepository.GetAsync(booking.PaymentId);

            if (payment == null)
            {
                throw new Exception("Payment not found for the booking.");
            }

            // Check payment status
            if (payment.Status != PaymentStatus.Successful)
            {
                throw new Exception("Refund cannot be requested for unsuccessful payments.");
            }

            // Update payment status to "Pending" for refund request
            payment.Status = PaymentStatus.RefundIssued;
            await _paymentRepository.Update(payment);

            // Perform refund process here (e.g., communicate with payment gateway)

            return true;
        }


        public double CalculateTotalPrice(int numberOfSeats, Flight flight)
        {
            double totalPrice = numberOfSeats * (flight?.BasePrice ?? 0);
            return totalPrice;

        }
    }
}
