using WebApiSimplyFly.Context;
using WebApiSimplyFly.Exceptions;
using WebApiSimplyFly.Interfaces;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Repositories
{
    public class PaymentDetailsRepository : IRepository<PaymentDetails, int>
    {
        newContext _context;
        ILogger<PaymentRepository> _logger;


        public PaymentDetailsRepository(newContext context, ILogger<PaymentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Task<PaymentDetails> Add(PaymentDetails items)
        {
            throw new NotImplementedException();
        }

        public async Task<PaymentDetails> Delete(int key)
        {
            var paymentDetails = await GetAsync(key);
            if (paymentDetails != null)
            {
                _context.Remove(paymentDetails);
                _context.SaveChanges();
                _logger.LogInformation($"PaymentDetails deleted with id {key}");
                return paymentDetails;
            }
            throw new NoSuchPaymentDetailsException();
        }

        public Task<PaymentDetails> GetAsync(int key)
        {
            throw new NotImplementedException();
        }

        public Task<List<PaymentDetails>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PaymentDetails> Update(PaymentDetails items)
        {
            throw new NotImplementedException();
        }
    }
}
