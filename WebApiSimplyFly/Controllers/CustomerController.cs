using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiSimplyFly.DTO;
using WebApiSimplyFly.Exceptions;
using WebApiSimplyFly.Interfaces;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IuserService _userService;
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(IuserService userService, ICustomerService customerService,  ILogger<CustomerController> logger)
        {
            _userService = userService;
            _customerService = customerService;
            _logger = logger;
        }

        [HttpGet("GetAllCustomers")]
        public async Task<ActionResult<List<Customer>>> GetAsync()
        {
            var users = await _customerService.GetAllCustomers();
            return Ok(users);
        }

        [HttpGet("GetCustomerByUsername")]
        public async Task<ActionResult<Customer>> GetCustomerByUsername(string username)
        {
            try
            {
                var customer = await _customerService.GetCustomersByUsername(username);
                return Ok(customer);
            }
            catch (NoSuchCustomerException nsce)
            {
                _logger.LogInformation(nsce.Message);
                return NotFound(nsce.Message);
            }

        }

        [Route("GetCustomerById")]
        [HttpGet]
        public async Task<ActionResult<Customer>> GetCustomerById(int customerId)
        {
            try
            {
                var customer = await _customerService.GetByIdCustomers(customerId);
                return Ok(customer);
            }
            catch (NoSuchCustomerException nsce)
            {
                _logger.LogInformation(nsce.Message);
                return NotFound(nsce.Message);
            }
        }

        [Route("UpdateCustomer")]
        [HttpPut]
        public async Task<ActionResult<Customer>> UpdateCustomer(UpdateCustomerDTO customerDTO)
        {
            try
            {
                var customer = await _customerService.UpdateCustomer(customerDTO);
                return customer;
            }
            catch (NoSuchCustomerException nsce)
            {
                _logger.LogInformation(nsce.Message);
                return NotFound(nsce.Message);
            }
        }

        [HttpDelete("DeleteCustomer/{customerId}")]
        public async Task<ActionResult<bool>> DeleteCustomer(int customerId)
        {
            var result = await _customerService.RemoveCustomer(customerId);
            if (result)
            {
                return Ok(result);
            }
            return NotFound("User not found.");
        }
    }
}
