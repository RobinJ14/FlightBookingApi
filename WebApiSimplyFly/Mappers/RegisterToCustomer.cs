using WebApiSimplyFly.DTO;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Mappers
{
    public class RegisterToCustomer
    {
        Customer customer;
        public RegisterToCustomer(RegisterCustomerDTO register)
        {
            customer = new Customer();
            customer.Name = register.Name;
            customer.Email = register.Email;
            customer.Phone = register.Phone;
            customer.Gender = register.Gender;
            customer.username = register.Username;
        }
        public Customer GetCustomer()
        {

            return customer;
        }
    }
}
