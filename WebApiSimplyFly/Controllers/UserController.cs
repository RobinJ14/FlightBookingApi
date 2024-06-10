using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiSimplyFly.DTO;
using WebApiSimplyFly.Exceptions;
using WebApiSimplyFly.Interfaces;

namespace WebApiSimplyFly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IuserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IuserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }




        [HttpPost("RegisterCustomer")]
        public async Task<ActionResult<LoginUserDTO>> RegisterCustomer(RegisterCustomerDTO user)
        {
            try
            {
                var result = await _userService.RegisterCustomer(user);
                return Ok(result);
            }
            catch (UserAlreadyPresentException uape)
            {
                _logger.LogError(uape.Message);
                return BadRequest(uape.Message);
            }

        }
        [Route("RegisterFlightOwner")]
        [HttpPost]
        public async Task<LoginUserDTO> RegisterFlightOwner(RegisterFlightOwnerDTO user)
        {
            var result = await _userService.RegisterFlightOwner(user);
            return result;
        }
        [Route("RegisterAdmin")]
        [HttpPost]
        public async Task<LoginUserDTO> RegisterAdmin(RegisterAdminDTO user)
        {
            var result = await _userService.RegisterAdmin(user);
            return result;
        }

        [Route("Login")]
        [HttpPost]
        public async Task<ActionResult<LoginUserDTO>> Login(LoginUserDTO user)
        {
            try
            {
                var result = await _userService.Login(user);
                return Ok(result);
            }
            catch (InvalidUserException iuse)
            {
                _logger.LogCritical(iuse.Message);
                return Unauthorized("Invalid username or password");
            }

        }
        [Route("UpdatePassword")]
        [HttpPut]
        public async Task<ActionResult<LoginUserDTO>> UpdatePassword(ForgotPasswordDTO forgotPasswordDTO)
        {
            try
            {
                var result = await _userService.UpdateUserPassword(forgotPasswordDTO);
                return Ok(result);
            }
            catch (NoSuchUserException nsue)
            {
                _logger.LogCritical(nsue.Message);
                return Unauthorized("Invalid username");
            }
        }
    }
}
