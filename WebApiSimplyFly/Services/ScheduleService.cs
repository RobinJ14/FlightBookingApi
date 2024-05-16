using WebApiSimplyFly.DTO;
using WebApiSimplyFly.Exceptions;
using WebApiSimplyFly.Interfaces;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Services
{
    public class ScheduleService : IScheduleService, IFlightSearchService
    {

        IRepository< Schedule,int> _scheduleRepository;
        IBookingService _bookingService;
        ILogger<ScheduleService> _logger;


       
        public ScheduleService(IRepository<Schedule,int> scheduleRepository, ILogger<ScheduleService> logger)
        {
            _scheduleRepository = scheduleRepository;
            _logger = logger;

        }
        public ScheduleService(IRepository<Schedule,int> scheduleRepository, IBookingService bookingService, ILogger<ScheduleService> logger)
        {
            _scheduleRepository = scheduleRepository;
            _bookingService = bookingService;
            _logger = logger;

        }
        public async Task<Schedule> AddSchedule(Schedule schedule)
        {

            var existingSchedules = await _scheduleRepository.GetAsync();
            bool isOverlap = existingSchedules.Any(e =>
    e.FlightNo == schedule.FlightNo &&
    ((schedule.DepartureTime >= e.DepartureTime && schedule.DepartureTime <= e.ArrivalTime) ||
    (schedule.ArrivalTime >= e.DepartureTime && schedule.ArrivalTime <= e.ArrivalTime) ||
    (e.DepartureTime >= schedule.DepartureTime && e.ArrivalTime <= schedule.ArrivalTime)));

            if (!isOverlap)
            {
                // If no overlap then only adding add the new schedule
                schedule = await _scheduleRepository.Add(schedule);
                return schedule;
            }
            throw new FlightScheduleBusyException();
        }

        public async Task<List<Schedule>> GetAllSchedules()
        {
            var schedules = await _scheduleRepository.GetAsync();
            return schedules;
        }

        public async Task<List<FlightScheduleDTO>> GetFlightSchedules(string flightNumber)
        {
            List<FlightScheduleDTO> flightSchedule = new List<FlightScheduleDTO>();

            var schedules = await _scheduleRepository.GetAsync();
            schedules = schedules.Where(e => e.FlightNo == flightNumber).ToList();

            flightSchedule = schedules.Select(e => new FlightScheduleDTO
            {
                FlightNumber = e.FlightNo,
                Id = e.ScheduleId,
                SourceAirport = e.Route?.SourceAirport?.Name + " ," + e.Route?.SourceAirport?.City,
                DestinationAirport = e.Route?.DestinationAirport?.Name + " ," + e.Route?.DestinationAirport?.City,
                Departure = e.DepartureTime,
                Arrival = e.ArrivalTime
            }).ToList();

            if (flightSchedule != null)
                return flightSchedule;
            else
                throw new NoSuchScheduleException();
        }

        public async Task<bool> RemoveSchedule(int Id)
        {
            var schedules = await _scheduleRepository.GetAsync(Id);
            if (schedules != null)
            {
                schedules = await _scheduleRepository.Delete(schedules.ScheduleId);
                return true;
            }
            throw new NoSuchScheduleException();
        }

        public async Task<int> RemoveSchedule(string flightNumber)
        {
            int removedScheduleCount = 0;
            var schedules = await _scheduleRepository.GetAsync();
            schedules = schedules.Where(e => e.FlightNo == flightNumber).ToList();
            foreach (var flight in schedules)
            {
                await _scheduleRepository.Delete(flight.ScheduleId);
                removedScheduleCount++;
            }
            return removedScheduleCount;
        }

        public async Task<int> RemoveSchedule(DateTime departureDate, int airportId)
        {
            int removedScheduleCount = 0;
            var schedules = await _scheduleRepository.GetAsync();
            schedules = schedules.Where(e => e.DepartureTime.Date == departureDate.Date &&
            e.Route.SourceAirportId == airportId).ToList();
            foreach (var flight in schedules)
            {
                await _scheduleRepository.Delete(flight.ScheduleId);
                removedScheduleCount++;
            }
            return removedScheduleCount;
        }

       

        public async Task<Schedule> UpdateScheduledFlight(int scheduleId, string flightNumber)
        {
            var schedule = await _scheduleRepository.GetAsync(scheduleId);
            if (schedule != null)
            {
                schedule.FlightNo = flightNumber;

                schedule = await _scheduleRepository.Update(schedule);
                return schedule;
            }
            throw new NoSuchScheduleException();
        }

        public async Task<Schedule> UpdateScheduledRoute(int scheduleId, int routeId)
        {
            var schedule = await _scheduleRepository.GetAsync(scheduleId);
            if (schedule != null)
            {
                schedule.RouteId = routeId;
                schedule = await _scheduleRepository.Update(schedule);
                return schedule;
            }
            throw new NoSuchScheduleException();
        }

        public async Task<Schedule> UpdateScheduledTime(int scheduleId, DateTime departure, DateTime arrival)
        {
            var schedule = await _scheduleRepository.GetAsync(scheduleId);
            if (schedule != null)
            {
                schedule.DepartureTime = departure;
                schedule.ArrivalTime = arrival;
                schedule = await _scheduleRepository.Update(schedule);
                return schedule;
            }
            throw new NoSuchScheduleException();
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
            if (searchFlightDto.SeatClass == "economy")
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
            var bookedSeatsTask = _bookingService.GetBookedSeatBySchedule(schedule);
            bookedSeatsTask.Wait();
            var bookedSeats = bookedSeatsTask.Result;

            int availableSeats = totalSeats - bookedSeats.Count();
            return availableSeats;
        }
    }
}
