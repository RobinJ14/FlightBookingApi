using WebApiSimplyFly.DTO;
using WebApiSimplyFly.Exceptions;
using WebApiSimplyFly.Interfaces;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Services
{
    public class Booking2
    {
        private readonly IRepository<Booking, int> _bookingRepository;
        private readonly IRepository<Flight, string> _flightRepository;
        private readonly IRepository<PassengerBooking, int> _passengerBookingRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly IRepository<Schedule, int> _scheduleRepository;
        private readonly IPassengerBookingRepository _passengerBookingsRepository;
        private readonly IBookingRepository _bookingsRepository;
        private readonly IRepository<Payment, int> _paymentRepository;
        private readonly IRepository<PaymentDetails, int> _paymentDetailsRepository;
        private readonly IRepository<Passenger, int> _passengerRepository;
        private readonly IRepository<Refund, int> _refundRepository;
        private readonly IRepository<History, int> _historyRepository;

        private readonly ILogger<BookingService> _logger;

        public Booking2(IRepository<History, int> historyRepository, IRepository<Refund, int> refundRepository, IRepository<Booking, int> bookingRepository, IRepository<Schedule, int> scheduleRepository, IRepository<PassengerBooking, int> passengerBookingRepository, IRepository<Flight, string> flightRepository, IBookingRepository bookingsRepository, ISeatRepository seatRepository, IPassengerBookingRepository passengerBookingRepository1, IRepository<Payment, int> paymentRepository, IRepository<PaymentDetails, int> paymentDetailsRepository, IRepository<Passenger, int> passengerRepository, ILogger<BookingService> logger)
        {
            _flightRepository = flightRepository;
            _bookingsRepository = bookingsRepository;
            _passengerBookingsRepository = passengerBookingRepository1;
            _bookingRepository = bookingRepository;
            _passengerBookingRepository = passengerBookingRepository;
            _seatRepository = seatRepository;
            _scheduleRepository = scheduleRepository;
            _paymentRepository = paymentRepository;
            _paymentDetailsRepository = paymentDetailsRepository;
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
                        SeatNo = seatDetail
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

            return true;



        }

        public async Task<bool> CheckSeatAvailablility(SeatCheckDTO seatCheckDTO)
        {
            var isSeatsAvailable = await _passengerBookingsRepository.CheckSeatsAvailabilityAsync(seatCheckDTO.ScheduleId, seatCheckDTO.SelectedSeats);
            if (!isSeatsAvailable)
            {
                return false;
            }
            return true;
        }

        public async Task<List<String>> GetBookedSeats(int ScheduleId)
        {
            var bookedSeats = await _passengerBookingsRepository.GetSeatNumbersForScheduleAsync(ScheduleId);
            return bookedSeats;
        }

        public async Task<List<SearchedFlightResultDTO>> SearchFlights(SearchFlightDTO searchFlight)
        {
            List<SearchedFlightResultDTO> searchResult = new List<SearchedFlightResultDTO>();
            var schedules = await _scheduleRepository.GetAsync();
            schedules = schedules.Where(e => e.DepartureTime.Date == searchFlight.DateOfJourney.Date
             && e.Route?.SourceAirport?.City == searchFlight.Origin
             && e.Route.DestinationAirport?.City == searchFlight.Destination
             && (AvailableSeats(e.Flight.TotalSeats, e.ScheduleId) > 0)).ToList();

            searchResult = schedules.Select(e => new SearchedFlightResultDTO
            {
                FlightNumber = e.FlightNo,
                Airline = e.Flight.FlightName,
                ScheduleId = e.ScheduleId,
                SourceAirport = e.Route.SourceAirport.City,
                DestinationAirport = e.Route.DestinationAirport.City,
                DepartureTime = e.DepartureTime,
                ArrivalTime = e.ArrivalTime,
                TotalPrice = CalculateTotalPrice(searchFlight, e.Flight.BasePrice)

            }).ToList();
            if (searchResult != null)
                return searchResult;
            else
                throw new NoFlightAvailableException();
        }

        public double CalculateTotalPrice(SearchFlightDTO searchFlightDto, double basePrice)
        {
            double totalPrice = 0;
            double seatPrice = 0;
            double adultSeatCost = 0;
            double childSeatCost = 0;
            if (searchFlightDto.SeatClass == "Economy")
                seatPrice = basePrice * 0.2;
            else if (searchFlightDto.SeatClass == "premiumEconomy")
                seatPrice = basePrice * 0.3;
            else
                seatPrice = basePrice * 0.4;

            adultSeatCost = basePrice + seatPrice + (basePrice * 0.3);
            childSeatCost = basePrice + seatPrice + (basePrice * 0.2);
            totalPrice = (adultSeatCost * searchFlightDto.Adult) + (childSeatCost * searchFlightDto.Child);

            return totalPrice;
        }


        public int AvailableSeats(int totalSeats, int schedule)
        {
            var bookedSeatsTask = GetBookedSeats(schedule);
            bookedSeatsTask.Wait();
            var bookedSeats = bookedSeatsTask.Result;

            int availableSeats = totalSeats - bookedSeats.Count();
            return availableSeats;
        }

        public async Task<List<Seat>> GetDetailsByFlight(string FlightNo)
        {
            return await _seatRepository.GetSeatDetailsByFlight(FlightNo);
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
            //Payment updatePayment = new Payment
            //{
            //    PaymentId = payment.PaymentId,
            //    PaymentDate = payment.PaymentDate,
            //    PaymentDetails = payment.PaymentDetails,
            //    Amount = payment.Amount,
            //    Status = Payment.PaymentStatus.RefundIssued
            //};
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
            return await _bookingRepository.Update(booking);

            History addHistory = new History
            {
                CustomerId = booking.CustomerId,
                BookingId = booking.BookingId,
                Action = History.ActionByCustomer.BookingCancelled,
                ActionDate = DateTime.Now
            };
            var history = _historyRepository.Add(addHistory);


        }

    }

}



