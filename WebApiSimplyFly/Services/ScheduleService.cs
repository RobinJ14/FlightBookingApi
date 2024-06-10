using WebApiSimplyFly.DTO;
using WebApiSimplyFly.Exceptions;
using WebApiSimplyFly.Interfaces;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Services
{
    public class ScheduleService : IScheduleService
    {

        IRepository<Schedule, int> _scheduleRepository;
        ILogger<ScheduleService> _logger;



        public ScheduleService(IRepository<Schedule, int> scheduleRepository, ILogger<ScheduleService> logger)
        {
            _scheduleRepository = scheduleRepository;
            _logger = logger;

        }

        public async Task<Schedule> AddSchedule(Schedule schedule)
        {

            var existingSchedules = await _scheduleRepository.GetAsync();
            bool isOverlap = existingSchedules.Any(e =>
    e.FlightId == schedule.FlightId &&
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

        public async Task<Schedule> GetScheduleById(int scheduleId)
        {

            var schedule = await _scheduleRepository.GetAsync(scheduleId);
            if (schedule != null)
                return schedule;
            else
                throw new NoSuchScheduleException();
        }

        public async Task<List<FlightScheduleDTO>> GetFlightSchedules(int flightNumber)
        {
            List<FlightScheduleDTO> flightSchedule = new List<FlightScheduleDTO>();

            var schedules = await _scheduleRepository.GetAsync();
            schedules = schedules.Where(e => e.FlightId == flightNumber).ToList();

            flightSchedule = schedules.Select(e => new FlightScheduleDTO
            {
                FlightId = e.FlightId,
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

        public async Task<List<FlightScheduleDTO>> GetSchedulesByOwner(int ownerId)
        {
            List<FlightScheduleDTO> flightSchedule = new List<FlightScheduleDTO>();

            var schedules = await _scheduleRepository.GetAsync();
            schedules = schedules.Where(e => e.Flight.FlightOwner.OwnerId == ownerId).ToList();

            flightSchedule = schedules.Select(e => new FlightScheduleDTO
            {
                FlightId = e.FlightId,
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

        public async Task<int> RemoveScheduleByFlight(int flightNumber)
        {
            int removedScheduleCount = 0;
            var schedules = await _scheduleRepository.GetAsync();
            schedules = schedules.Where(e => e.FlightId == flightNumber).ToList();
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



        public async Task<Schedule> UpdateSchedule(Schedule schedule)
        {
            var updateSchedule = await _scheduleRepository.GetAsync(schedule.ScheduleId);
            if (updateSchedule != null)
            {
                updateSchedule.RouteId = schedule.RouteId;
                updateSchedule.FlightId = schedule.FlightId;
                updateSchedule.DepartureTime = schedule.DepartureTime;
                updateSchedule.ArrivalTime = schedule.ArrivalTime;

                updateSchedule = await _scheduleRepository.Update(schedule);
                return updateSchedule;
            }
            throw new NoSuchScheduleException();
        }





    }
}
