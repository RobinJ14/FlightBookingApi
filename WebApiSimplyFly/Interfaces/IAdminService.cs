using WebApiSimplyFly.DTO;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Interfaces
{
    public interface IAdminService
    {

        public Task<Admin> GetAdminByUsername(string username);
        public Task<Admin> UpdateAdmin(UpdateAdminDTO admin);
        public Task<User> DeleteUser(string username);
        public Task<bool> RemoveAdmin(int id);

    }
}
