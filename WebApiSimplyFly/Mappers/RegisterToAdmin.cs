using WebApiSimplyFly.DTO;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Mappers
{
    public class RegisterToAdmin
    {
        Admin admin;
        public RegisterToAdmin(RegisterAdminDTO register)
        {
            admin = new Admin();
            admin.Name = register.Name;
            admin.Email = register.Email;
            admin.Phone = register.Phone;
            admin.Address = register.Address;
            admin.Username = register.Username;
        }
        public Admin GetAdmin()
        {

            return admin;
        }
    }
}
