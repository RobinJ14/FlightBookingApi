﻿using WebApiSimplyFly.DTO;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Interfaces
{
    public interface ICustomerService
    {
        public Task<Customer> AddCustomer(Customer customer);
        public Task<bool> RemoveCustomer(int Id);
        public Task<List<Customer>> GetAllCustomers();
        public Task<Customer> GetByIdCustomers(int id);
        public Task<Customer> GetCustomersByUsername(string username);
        public Task<Customer> UpdateCustomer(UpdateCustomerDTO customer);
    }
}
