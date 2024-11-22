using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace WebApplication5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }
        
        [Authorize]
        [HttpPost("set-admin")]
        public async Task<IActionResult> SetUserRoleToAdmin([FromBody] SetUserRoleToAdminRequest request)
        {
            try
            {
                var result = await _userService.SetUserRoleToAdmin(request.Username);
                if (result)
                {
                    return Ok("User role updated to Admin.");
                }
                return NotFound("User not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}