using Microsoft.EntityFrameworkCore;
using WebApiSimplyFly.Context;
using WebApiSimplyFly.Exceptions;
using WebApiSimplyFly.Interfaces;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Repositories
{
    public class ScheduleRepository : IRepository<Schedule, int>
    {
        newContext _context;
        private readonly ILogger<ScheduleRepository> _logger;
        
        public ScheduleRepository(newContext context, ILogger<ScheduleRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Schedule> Add(Schedule items)
        {
            _context.Add(items);
            _context.SaveChanges();
            _logger.LogInformation("Schedule added with scheduleId" + items.ScheduleId);
            return items;
        }

        public async Task<Schedule> Delete(int key)
        {
            var schedule = await GetAsync(key);
            if (schedule != null)
            {
                _context.Remove(schedule);
                _context.SaveChanges();
                _logger.LogInformation("Schedule deleted with scheduleId" + key);
                return schedule;
            }
            throw new NoSuchScheduleException();
        }

        public async Task<Schedule> GetAsync(int key)
        {
            var schedules = await GetAsync();
            var schedule = schedules.FirstOrDefault(e => e.ScheduleId == key);
            if (schedule != null)
            {
                return schedule;
            }
            throw new NoSuchScheduleException();
        }

        public async Task<List<Schedule>> GetAsync()
        {
            var schedules = await _context.Schedules.AsNoTracking()
                                    .Include(e => e.Route)
                                    .Include(e => e.Flight)
                                    .Include(e=> e.Flight.FlightOwner)
                                    .Include(e => e.Route.SourceAirport)
                                    .Include(e => e.Route.DestinationAirport)
                                    .ToListAsync();
            return schedules;
        }

        public async Task<Schedule> Update(Schedule items)
        {
            var schedule = await GetAsync(items.ScheduleId);
            if (schedule != null)
            {
                _context.Entry<Schedule>(items).State = EntityState.Modified;
                _context.SaveChanges();
                _logger.LogInformation("Schedule updated with scheduleId" + items.ScheduleId);
                return schedule;
            }
            throw new NoSuchScheduleException();
        }
    }
}
