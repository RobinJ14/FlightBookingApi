using Microsoft.EntityFrameworkCore;
using WebApiSimplyFly.Context;
using WebApiSimplyFly.Exceptions;
using WebApiSimplyFly.Interfaces;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Repositories
{
    public class HistoryRepository : IRepository<History, int>
    {
        private readonly newContext _context;
        private readonly ILogger<HistoryRepository> _logger;


        public HistoryRepository(newContext context, ILogger<HistoryRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<History> Add(History items)
        {
            _context.Add(items);
            _context.SaveChanges();
            _logger.LogInformation("History added " + items.HistoryId);
            return items;
        }

        public async Task<History> Delete(int key)
        {
            var history = await GetAsync(key);
            if (history != null)
            {
                _context.Remove(history);
                _context.SaveChanges();
                _logger.LogInformation("Hsitory deleted with History number" + key);
                return history;
            }
            throw new NoSuchHistoryException();
        }

        public async Task<History> GetAsync(int key)
        {
            var histories = await GetAsync();
            var history = histories.FirstOrDefault(e => e.HistoryId == key);
            if (history != null)
            {
                return history;
            }
            throw new NoSuchHistoryException();
        }

        public async Task<List<History>> GetAsync()
        {
            var histories = _context.Histories.ToList();
            return histories;
        }

        public async Task<History> Update(History items)
        {
            var history = await GetAsync(items.HistoryId);
            if (history != null)
            {
                _context.Entry<History>(items).State = EntityState.Modified;
                _context.SaveChanges();
                _logger.LogInformation("History updated with History Id" + items.HistoryId);
                return history;
            }
            throw new NoSuchHistoryException();
        }
    }
}
