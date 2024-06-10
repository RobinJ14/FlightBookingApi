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
        private readonly IRepository<PassengerBooking,int> _passengerBookingRepository;
        private readonly IPassengerBookingRepository _passengerBookingsRepository;
        private readonly IBookingRepository _bookingsRepository;
        private readonly IRepository<Payment,int> _paymentRepository;
        private readonly IRepository<Passenger, int> _passengerRepository;
        private readonly IRepository<Refund, int> _refundRepository;
        private readonly IRepository<History, int> _historyRepository;
        private readonly ILogger<BookingService> _logger;

        public BookingService(IRepository<Passenger, int> passengerRepository,
            IRepository<History, int> historyRepository, IRepository<Refund, int> refundRepository, 
            IRepository<Booking,int> bookingRepository, IRepository<PassengerBooking,int> passengerBookingRepository,
             IBookingRepository bookingsRepository, 
            IPassengerBookingRepository passengerBookingRepository1, IRepository<Payment,int> paymentRepository, ILogger<BookingService> logger)
        {
            _bookingsRepository = bookingsRepository;
            _passengerBookingsRepository = passengerBookingRepository1;
            _bookingRepository = bookingRepository;
            _passengerBookingRepository = passengerBookingRepository;
            _paymentRepository = paymentRepository;
            _passengerRepository = passengerRepository;
            _refundRepository = refundRepository;
            _historyRepository = historyRepository;
            _logger = logger;
        }

        public async Task<bool> CreateBookingAsync(BookingRequestDTO bookingRequest)
        {

            Payment payment = new Payment
            {
                Amount = bookingRequest.Price,
                PaymentDate = DateTime.Now,
                Status = Payment.PaymentStatus.Successful,
                PaymentDetails = bookingRequest.PaymentDetails,
            };

            var addedPayment = await _paymentRepository.Add(payment);

            Booking booking = new Booking
            {
                ScheduleId = bookingRequest.ScheduleId,
                CustomerId = bookingRequest.UserId,
                BookingTime = DateTime.Now,
                TotalPrice = bookingRequest.Price,
                PaymentId = addedPayment.PaymentId,
                SeatCount = bookingRequest.SelectedSeats.Count,
                bookingStatus = Booking.BookingStatus.Successful,
            };

            var addedBooking = await _bookingRepository.Add(booking);

            int index = 0;
            foreach (var passengerId in bookingRequest.PassengerIds)
            {
                var seatDetail = bookingRequest.SelectedSeats.ElementAtOrDefault(index);
                if (seatDetail != null)
                {
                    var passengerBooking = new PassengerBooking
                    {
                        BookingId = addedBooking.BookingId,
                        PassengerId = passengerId,
                        SeatId = seatDetail
                    };
                    await _passengerBookingRepository.Add(passengerBooking);
                    index++;
                }
                else
                {
                    throw new Exception("Not enough seats available for all passengers.");
                }
            }

            History addHistory = new History
            {
                CustomerId = addedBooking.CustomerId,
                BookingId = addedBooking.BookingId,
                Action = History.ActionByCustomer.BookingSuccessful,
                ActionDate = DateTime.Now
            };
            var history = _historyRepository.Add(addHistory);

            return addedBooking != null && addedPayment != null;
        }

        public async Task<Booking> CancelBookingAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetAsync(bookingId);

            if (booking == null)
            {
                throw new NoSuchBookingException();
            }


            var passengerBookings = await _passengerBookingsRepository.GetPassengerBookingsAsync(bookingId);
            foreach (var passengerBooking in passengerBookings)
            {
                await _passengerBookingRepository.Delete(passengerBooking.PassengerBookingId);
                await _passengerRepository.Delete(passengerBooking.PassengerId);

            }

            var payment = await _paymentRepository.GetAsync(booking.PaymentId);
            payment.Status = Payment.PaymentStatus.RefundIssued;

            await _paymentRepository.Update(payment);


            Refund addRefund = new Refund
            {
                BookingId = bookingId,
                RefundDate = DateTime.Now,
                RefundAmount = booking.TotalPrice,
                Status = Refund.RefundStatus.RefundIssued
            };
            var refund = _refundRepository.Add(addRefund);

            booking.bookingStatus = Booking.BookingStatus.Cancelled;

            History addHistory = new History
            {
                CustomerId = booking.CustomerId,
                BookingId = booking.BookingId,
                Action = History.ActionByCustomer.BookingCancelled,
                ActionDate = DateTime.Now
            };
            var history = _historyRepository.Add(addHistory);

            return await _bookingRepository.Update(booking);

        }

        //not sure use
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

        public async Task<Booking> GetBookingByIdAsync(int bookingId)
        {
            return await _bookingRepository.GetAsync(bookingId);
        }



        public async Task<List<Booking>> GetBookingByFlight(int flightNumber)
        {
            var bookings = await _bookingRepository.GetAsync();
            bookings = bookings.Where(e => e.Schedule.FlightId == flightNumber).ToList();
            if (bookings != null)
            {
                return bookings;
            }
            throw new NoSuchBookingException();
        }

        public async Task<List<Booking>> GetBookingByOwnerId(int ownerId)
        {
            var bookings = await _bookingRepository.GetAsync();
            bookings = bookings.Where(e => e.Schedule.Flight.FlightOwner.OwnerId == ownerId).ToList();
            if (bookings != null)
            {
                return bookings;
            }
            throw new NoSuchBookingException();
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


        //get full details with passengers
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

        //get only booking details basic
        public async Task<IEnumerable<Booking>> GetBookingByCustomer(int customerId)
        {
            var bookings = await _bookingsRepository.GetBookingsByCustomerIdAsync(customerId);
            if (bookings != null)
            {
                //bookings = bookings.Where(e=>e.Schedule.DepartureTime<DateTime.Now);
                return bookings;
            }
            throw new NoSuchBookingException();
        }

        //to get passenger details after above method
        public async Task<List<PassengerBooking>> GetPassengerBookingByBookingId(int bookingId)
        {
            var bookings = await _passengerBookingRepository.GetAsync();
            bookings = bookings.Where(e => e.Booking.BookingId == bookingId).ToList();
            if (bookings != null)
            {
                return bookings;
            }
            throw new NoSuchCustomerException();
        }

        

        //not sure
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


        



       

       
    }
}
