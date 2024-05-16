using WebApiSimplyFly.DTO;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Interfaces
{
    public interface IScheduleService
    {
        public Task<Schedule> AddSchedule(Schedule schedule);
        public Task<bool> RemoveSchedule(int Id);
        public Task<int> RemoveSchedule(string flightNumber);
        public Task<int> RemoveSchedule(DateTime departureDate, int airportId);
        public Task<List<Schedule>> GetAllSchedules();
        public Task<List<FlightScheduleDTO>> GetFlightSchedules(string flightNumber);
        public Task<Schedule> UpdateScheduledFlight(int scheduleId, string flightNumber);

        public Task<Schedule> UpdateScheduledRoute(int scheduleId, int routeId);
        public Task<Schedule> UpdateScheduledTime(int scheduleId, DateTime departure, DateTime arrival);
    }
}
