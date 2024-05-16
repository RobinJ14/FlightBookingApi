using Microsoft.EntityFrameworkCore;
using WebApiSimplyFly.Context;
using WebApiSimplyFly.Exceptions;
using WebApiSimplyFly.Interfaces;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Repositories
{
    public class CustomerRepository : IRepository<Customer, int>
    {
        private readonly newContext _context;
        private readonly ILogger<CustomerRepository> _logger;
        
        public CustomerRepository(newContext context, ILogger<CustomerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Customer> Add(Customer item)
        {
            _context.Add(item);
            _context.SaveChanges();
            _logger.LogInformation($"Customer added with id {item.CustomerId}");
            return item;
        }

        public async Task<Customer> Delete(int key)
        {
            var customer = await GetAsync(key);
            if (customer != null)
            {
                _context.Remove(customer);
                _context.SaveChanges();
                _logger.LogInformation($"Customer deleted with Customer id {key}");
                return customer;
            }
            throw new NoSuchCustomerException();
        }

        public async Task<Customer> GetAsync(int key)
        {
            var customers = await GetAsync();
            var customer = customers.FirstOrDefault(e => e.CustomerId == key);
            if (customer != null)
            {
                return customer;
            }
            throw new NoSuchCustomerException();
        }

        public async Task<List<Customer>> GetAsync()
        {
            var customers = _context.Customers.ToList();
            return customers;
        }

        public async Task<Customer> Update(Customer item)
        {
            var customer = await GetAsync(item.CustomerId);
            if (customer != null)
            {
                _context.Entry<Customer>(item).State = EntityState.Modified;
                _context.SaveChanges();
                _logger.LogInformation($"Customer updated with id {item.CustomerId}");
                return customer;
            }
            throw new NoSuchCustomerException();
        }
    }
}
