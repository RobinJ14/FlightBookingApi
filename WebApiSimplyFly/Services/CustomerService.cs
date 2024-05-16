using WebApiSimplyFly.DTO;
using WebApiSimplyFly.Exceptions;
using WebApiSimplyFly.Interfaces;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Services
{
    public class CustomerService:ICustomerService
    {
        private readonly IRepository<User,string> _userRepository;
        private readonly IRepository<Customer,int> _customerRepository;
        private readonly ILogger<CustomerService> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerRepository"></param>
        /// <param name="userRepository"></param>
        /// <param name="logger"></param>
        public CustomerService(IRepository<Customer,int> customerRepository, IRepository<User,string> userRepository, ILogger<CustomerService> logger)
        {
            _userRepository = userRepository;
            _customerRepository = customerRepository;
            _logger = logger;

        }

        public async Task<Customer> AddCustomer(Customer customer)
        {
            return await _customerRepository.Add(customer);
        }

        public async Task<List<Customer>> GetAllCustomers()
        {
            return await _customerRepository.GetAsync();
        }

        public async Task<Customer> GetByIdCustomers(int id)
        {
            var customer = await(_customerRepository.GetAsync(id));
            if (customer != null)
            {
                return customer;
            }
            throw new NoSuchCustomerException();
        }

        public async Task<Customer> GetCustomersByUsername(string username)
        {
            var customers = await _customerRepository.GetAsync();
            var customer = customers.FirstOrDefault(e => e.username == username);
            if (customer != null)
            {
                return customer;
            }
            throw new NoSuchCustomerException();
        }

        public async Task<bool> RemoveCustomer(int id)
        {

            var cust = await _customerRepository.Delete(id);
            if (cust != null)
            {
                var user = await _userRepository.Delete(cust.username);
                await _userRepository.Delete(cust.username);
                _logger.LogInformation("Customer removed with id " + id);
                return true;
            }
            return false;
        }

        public async Task<Customer> UpdateCustomer(UpdateCustomerDTO customer)
        {
            var customers = await _customerRepository.GetAsync(customer.UserId);
            if (customer != null)
            {
                customers.Name = customer.Name;
                customers.Email = customer.Email;
                customers.Phone = customer.Phone;
                customers = await _customerRepository.Update(customers);
                return customers;
            }
            throw new NoSuchCustomerException();
        }

        public async Task<Customer> UpdateCustomerEmail(int id, string Email)
        {
            var cust = await _customerRepository.GetAsync(id);
            if (cust != null)
            {
                cust.Email = Email;
                cust = await _customerRepository.Update(cust);
                return cust;
            }
            return null;
        }
    }
}
