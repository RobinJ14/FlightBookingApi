using System.Security.Cryptography;
using System.Text;
using WebApiSimplyFly.DTO;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Mappers
{
    public class RegisterToUser
    {
        User user;
        public RegisterToUser(RegisterCustomerDTO register)
        {

            user = new User();
            user.Username = register.Username;
            user.Role = register.Role;

            GetPassword(register.Password);
        }

        public RegisterToUser(RegisterAdminDTO register)
        {

            user = new User();
            user.Username = register.Username;
            user.Role = register.Role;

            GetPassword(register.Password);
        }
        public RegisterToUser(RegisterFlightOwnerDTO register)
        {

            user = new User();
            user.Username = register.Username;
            user.Role = register.Role;

            GetPassword(register.Password);
        }

        public RegisterToUser(ForgotPasswordDTO userDTO)
        {

            user = new User();
            user.Username = userDTO.Username;

            GetPassword(userDTO.Password);
        }

        void GetPassword(string password)
        {
            HMACSHA512 hmac = new HMACSHA512();
            user.Key = hmac.Key;
            user.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
        public User GetUser()
        {
            return user;
        }
    }
}
