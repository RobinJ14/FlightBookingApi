using Microsoft.EntityFrameworkCore;
using WebApiSimplyFly.Context;
using WebApiSimplyFly.Exceptions;
using WebApiSimplyFly.Interfaces;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Repositories
{
    public class RefundRepository : IRepository<Refund, int>
    {
        private readonly newContext _context;
        private readonly ILogger<RefundRepository> _logger;


        public RefundRepository(newContext context, ILogger<RefundRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Refund> Add(Refund items)
        {
            _context.Add(items);
            _context.SaveChanges();
            _logger.LogInformation($"Refund added with id {items.RefundId}");
            return items;
        }

        public async Task<Refund> Delete(int key)
        {
            var refund = await GetAsync(key);
            if (refund != null)
            {
                _context.Remove(refund);
                _context.SaveChanges();
                _logger.LogInformation($"Refund deleted with id {key}");
                return refund;
            }
            throw new NoSuchRefundException();
        }

        public async Task<Refund> GetAsync(int key)
        {
            var refunds = await GetAsync();
            var refund = refunds.FirstOrDefault(e => e.RefundId == key);
            if (refund != null)
            {
                return refund;
            }
            throw new NoSuchRefundException();
        }

        public async Task<List<Refund>> GetAsync()
        {
            var refunds = _context.Refunds.ToList();
            return refunds;
        }

        public async Task<Refund> Update(Refund items)
        {
            var refund = await GetAsync(items.RefundId);
            if (refund != null)
            {
                _context.Entry<Refund>(items).State = EntityState.Modified;
                _context.SaveChanges();
                _logger.LogInformation("Refund updated with refundId" + items.RefundId);
                return refund;
            }
            throw new NoSuchRefundException();
        }
    }
}
