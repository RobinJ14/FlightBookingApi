using System.Security.Cryptography;
using System.Text;
using WebApiSimplyFly.DTO;
using WebApiSimplyFly.Exceptions;
using WebApiSimplyFly.Interfaces;
using WebApiSimplyFly.Mappers;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Services
{
    public class UserService : IuserService
    {
        private readonly IRepository<User,string> _userRepository;
        private readonly IRepository<Customer,int> _customerRepository;
        private readonly IRepository<FlightOwner, int> _flightownerRepository;
        private readonly IRepository<Admin, int> _adminRepository;
        private readonly ILogger<UserService> _logger;
        private readonly ITokenService _tokenService;


        public UserService(IRepository<User, string> userRepository, IRepository<Customer,int> customerRepository, IRepository<FlightOwner, int> flightOwnerRepository, IRepository<Admin, int> adminRepository, ITokenService tokenService, ILogger<UserService> logger)
        {

            _userRepository = userRepository;
            _customerRepository = customerRepository;
            _flightownerRepository = flightOwnerRepository;
            _adminRepository = adminRepository;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<LoginUserDTO> Login(LoginUserDTO user)
        {
            var myUSer = await _userRepository.GetAsync(user.Username);
            if (myUSer == null)
            {
                throw new InvalidUserException();
            }
            var userPassword = GetPasswordEncrypted(user.Password, myUSer.Key);
            var checkPasswordMatch = ComparePasswords(myUSer.Password, userPassword);
            if (checkPasswordMatch)
            {
                user.Password = "";
                user.Role = myUSer.Role;
                user.Token = await _tokenService.GenerateToken(user);
                return user;
            }
            throw new InvalidUserException();
        }

        public bool ComparePasswords(byte[] password, byte[] userPassword)
        {
            for (int i = 0; i < password.Length; i++)
            {
                if (password[i] != userPassword[i])
                    return false;
            }
            return true;
        }

        private byte[] GetPasswordEncrypted(string password, byte[] key)
        {
            HMACSHA512 hmac = new HMACSHA512(key);
            var userpassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return userpassword;
        }

        public async Task<LoginUserDTO> RegisterAdmin(RegisterAdminDTO user)
        {
            var users = await _userRepository.GetAsync();
            var checkUser = users.FirstOrDefault(e => e.Username == user.Username);
            if (checkUser == null)
            {
                User myUser = new RegisterToUser(user).GetUser();
                myUser = await _userRepository.Add(myUser);
                Admin admin = new RegisterToAdmin(user).GetAdmin();
                await _adminRepository.Add(admin);
                LoginUserDTO result = new LoginUserDTO
                {
                    Username = myUser.Username,
                    Role = myUser.Role,

                };
                return result;
            }
            throw new UserAlreadyPresentException();
        }

        public async Task<LoginUserDTO> RegisterCustomer(RegisterCustomerDTO user)
        {
            User myuser = new RegisterToUser(user).GetUser();
            myuser = await _userRepository.Add(myuser);
            Customer customer = new RegisterToCustomer(user).GetCustomer();
            customer = await _customerRepository.Add(customer);
            LoginUserDTO result = new LoginUserDTO
            {
                Username = myuser.Username,
                Role = myuser.Role,

            };
            return result;
        }

        public async Task<LoginUserDTO> RegisterFlightOwner(RegisterFlightOwnerDTO user)
        {
            User myuser = new RegisterToUser(user).GetUser();
            myuser = await _userRepository.Add(myuser);
            FlightOwner flightOwner = new RegisterToFlightOwner(user).GetFlightOwner();
            await _flightownerRepository.Add(flightOwner);
            LoginUserDTO result = new LoginUserDTO
            {
                Username = myuser.Username,
                Role = myuser.Role,

            };
            return result;
        }

        public async Task<LoginUserDTO> UpdateUserPassword(ForgotPasswordDTO userDTO)
        {
            User user = new RegisterToUser(userDTO).GetUser();
            var findUser = await _userRepository.GetAsync(userDTO.Username);
            if (findUser != null)
            {
                findUser.Password = user.Password;
                findUser.Key = user.Key;
                findUser = await _userRepository.Update(findUser);
                LoginUserDTO result = new LoginUserDTO
                {
                    Username = findUser.Username,
                    Role = findUser.Role,
                };
                return result;
            }
            throw new NoSuchUserException();
        }
    }
}
