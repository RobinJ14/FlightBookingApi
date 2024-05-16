using WebApiSimplyFly.DTO;

namespace WebApiSimplyFly.Interfaces
{
    public interface ITokenService
    {
        public Task<string> GenerateToken(LoginUserDTO user);

    }
}
