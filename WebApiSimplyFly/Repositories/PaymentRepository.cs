using Microsoft.EntityFrameworkCore;
using WebApiSimplyFly.Context;
using WebApiSimplyFly.Exceptions;
using WebApiSimplyFly.Interfaces;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Repositories
{
    public class PaymentRepository : IRepository<Payment, int>
    {

        newContext _context;
        ILogger<PaymentRepository> _logger;

      
        public PaymentRepository(newContext context, ILogger<PaymentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Payment> Add(Payment items)
        {
            _context.Add(items);
            _context.SaveChanges();
            _logger.LogInformation($"Payment added with id {items.PaymentId}");
            return items;
        }

        public async Task<Payment> Delete(int key)
        {
            var payment = await GetAsync(key);
            if (payment != null)
            {
                _context.Remove(payment);
                _context.SaveChanges();
                _logger.LogInformation($"Payment added with id {key}");
                return payment;
            }
            throw new NoSuchPaymentException();
        }

        public async Task<Payment> GetAsync(int key)
        {
            var payments = await GetAsync();
            var payment = payments.FirstOrDefault(e => e.PaymentId == key);
            if (payment != null)
            {
                return payment;
            }
            throw new NoSuchPaymentException();
        }

        public async Task<List<Payment>> GetAsync()
        {
            var payments = _context.Payments.ToList();
            return payments;
        }

        public async Task<Payment> Update(Payment items)
        {
            var payment = await GetAsync(items.PaymentId);
            if (payment != null)
            {
                _context.Entry<Models.Payment>(items).State = EntityState.Modified;
                _context.SaveChanges();
                _logger.LogInformation($"Payment updated with id {items.PaymentId}");
                return payment;
            }
            throw new NoSuchPaymentException();
        }
    }
}
