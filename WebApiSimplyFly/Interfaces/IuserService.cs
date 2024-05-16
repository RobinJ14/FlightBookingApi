using WebApiSimplyFly.DTO;

namespace WebApiSimplyFly.Interfaces
{
    public interface IuserService
    {
        public Task<LoginUserDTO> Login(LoginUserDTO user);
        public Task<LoginUserDTO> RegisterAdmin(RegisterAdminDTO user);

        public Task<LoginUserDTO> RegisterFlightOwner(RegisterFlightOwnerDTO user);
        public Task<LoginUserDTO> RegisterCustomer(RegisterCustomerDTO user);
        public Task<LoginUserDTO> UpdateUserPassword(ForgotPasswordDTO userDTO);
    }
}
