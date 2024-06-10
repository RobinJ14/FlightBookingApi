using WebApiSimplyFly.DTO;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Interfaces
{
    public interface IScheduleService
    {
        public Task<Schedule> AddSchedule(Schedule schedule);
        public Task<bool> RemoveSchedule(int Id);
        public Task<int> RemoveScheduleByFlight(int flightNumber);
        public Task<int> RemoveSchedule(DateTime departureDate, int airportId);
        public Task<List<Schedule>> GetAllSchedules();
        public Task<List<FlightScheduleDTO>> GetFlightSchedules(int flightNumber);
        public Task<Schedule> GetScheduleById(int ScheduleId);

        public Task<Schedule> UpdateSchedule(Schedule schedule);

       
    }
}
