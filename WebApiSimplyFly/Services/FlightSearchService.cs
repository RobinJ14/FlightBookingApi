using WebApiSimplyFly.DTO;
using WebApiSimplyFly.Exceptions;
using WebApiSimplyFly.Interfaces;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Services
{
    public class FlightSearchService : IFlightSearchService
    {
        private readonly ISeatRepository _seatRepository;
        private readonly IRepository<Schedule, int> _scheduleRepository;
        private readonly IPassengerBookingRepository _passengerBookingsRepository;
        private readonly IRepository<PassengerBooking, int> _passengerBookingRepository;




        public FlightSearchService(IRepository<Schedule, int> scheduleRepository, ISeatRepository seatRepository, IPassengerBookingRepository passengerBookingRepository1, IRepository<PassengerBooking, int> passengerBookingRepository)
        {
            _seatRepository = seatRepository;
            _scheduleRepository = scheduleRepository;
            _passengerBookingsRepository = passengerBookingRepository1;
            _passengerBookingRepository = passengerBookingRepository;
            _passengerBookingRepository = passengerBookingRepository;
        }

        //to get all flight Seats
        public async Task<List<Seat>> GetSeatDetailsByFlight(int FlightNo)
        {
            return await _seatRepository.GetSeatDetailsByFlight(FlightNo);
        }

        //to get booked seats for a schedule
        public async Task<List<int>> GetBookedSeatBySchedule(int scheduleID)
        {
            var bookings = await _passengerBookingRepository.GetAsync();
            var bookedSeats = bookings.Where(e => e.Booking.ScheduleId == scheduleID)
                .Select(e => e.SeatId).ToList();
            if (bookedSeats != null)
            {
                return bookedSeats;
            }
            throw new NoSuchBookingException(); 
        }


        //search
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
                FlightId = e.FlightId,
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
            else if (searchFlightDto.SeatClass == "PremiumEconomy")
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
            var bookedSeatsTask = GetBookedSeatBySchedule(schedule);
            bookedSeatsTask.Wait();
            var bookedSeats = bookedSeatsTask.Result;

            int availableSeats = totalSeats - bookedSeats.Count();
            return availableSeats;
        }


        //not sure use
        public async Task<bool> CheckSeatAvailablility(SeatCheckDTO seatCheckDTO)
        {
            var isSeatsAvailable = await _passengerBookingsRepository.CheckSeatsAvailabilityAsync(seatCheckDTO.ScheduleId, seatCheckDTO.SelectedSeats);
            if (!isSeatsAvailable)
            {
                return false;
            }
            return true;
        }
    }
}
