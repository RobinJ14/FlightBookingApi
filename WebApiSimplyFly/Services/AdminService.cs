using WebApiSimplyFly.DTO;
using WebApiSimplyFly.Exceptions;
using WebApiSimplyFly.Interfaces;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Services
{
    public class AdminService : IAdminService
    {
        private readonly IRepository<Admin,int> _adminRepository;
        private readonly IRepository< User,String> _userRepository;
        private readonly ILogger<AdminService> _logger;

       
        public AdminService(IRepository< Admin,int> adminRepository, ILogger<AdminService> logger)
        {
            _adminRepository = adminRepository;
            _logger = logger;
        }
        public AdminService(IRepository<Admin,int> adminRepository, ILogger<AdminService> logger, IRepository<User,string> userRepository)
        {
            _adminRepository = adminRepository;
            _logger = logger;
            _userRepository = userRepository;
        }
        public async Task<User> DeleteUser(string username)
        {
            var users = await _userRepository.GetAsync();
            var user = users.FirstOrDefault(e => e.Username == username);
            if (user != null)
            {
                user = await _userRepository.Delete(username);
                return user;
            }
            throw new NoSuchUserException();
        }

        public async Task<Admin> GetAdminByUsername(string username)
        {
            var admins = await _adminRepository.GetAsync();
            var admin = admins.FirstOrDefault(e => e.Username == username);
            if (admin != null) return admin;
            throw new NoSuchAdminException();
        }

        public async Task<Admin> UpdateAdmin(UpdateAdminDTO admin)
        {
            var admins = await _adminRepository.GetAsync(admin.AdminId);
            if (admins != null)
            {
                admins.Name = admin.Name;
                admins.Email = admin.Email;
                admins.Address = admin.Address;
                admins.Phone = admin.Phone;
                admins = await _adminRepository.Update(admins);
                return admins;
            }
            throw new NoSuchAdminException();
        }
    }
}
