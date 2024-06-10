using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiSimplyFly.DTO;
using WebApiSimplyFly.Exceptions;
using WebApiSimplyFly.Interfaces;
using WebApiSimplyFly.Models;

namespace WebApiSimplyFly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IAdminService adminService, ILogger<AdminController> logger)
        {
            _adminService = adminService;
            _logger = logger;
        }

        [HttpGet("GetAdminByUsername")]
        public async Task<ActionResult<Admin>> GetAdminByUsername([FromQuery] string username)
        {
            var result = await _adminService.GetAdminByUsername(username);
            return Ok(result);
        }

        
        [HttpPut]
        public async Task<ActionResult<Admin>> UpdateAdmin(UpdateAdminDTO adminDTO)
        {
            try
            {
                var admin = await _adminService.UpdateAdmin(adminDTO);
                return admin;
            }
            catch (NoSuchAdminException nsae)
            {
                _logger.LogInformation(nsae.Message);
                return NotFound(nsae.Message);
            }
        }

        [Route("DeleteUserByUsername")]
        [HttpDelete]
        public async Task<ActionResult<User>> DeleteUserByUsername(string username)
        {
            try
            {
                var user = await _adminService.DeleteUser(username);
                return Ok(user);
            }
            catch (NoSuchUserException nsue)
            {
                _logger.LogError(nsue.Message);
                return NotFound(nsue.Message);
            }
        }

        [Route("DeleteAdminById")]
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteAdminById(int Id)
        {
            try
            {
                var user = await _adminService.RemoveAdmin(Id);
                return Ok(user);
            }
            catch (NoSuchUserException nsue)
            {
                _logger.LogError(nsue.Message);
                return NotFound(nsue.Message);
            }
        }

    }
}
