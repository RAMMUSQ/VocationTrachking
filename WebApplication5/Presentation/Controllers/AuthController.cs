using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Core.Entities;
using WebApplication5.Models;
using WebApplication5.Services;

namespace WebApplication5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var result = await _userService.RegisterAsync(model);
            if (!result.IsSuccess)
            {
                return BadRequest(new { Message = result.ErrorMessage });
            }
            return Ok(new { Message = "User registered successfully." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userService.ValidateUserAsync(model.Username, model.Password);

            if (user == null)
                return Unauthorized(new { Message = "Invalid credentials" });

            var token = _tokenService.GenerateToken(user);

            // Token'ı veritabanına kaydet
            var tokenEntity = new Token
            {
                UserId = user.Id,
                Value = token,
                Expiration = DateTime.UtcNow.AddHours(1) // Token geçerlilik süresi
            };

            await _tokenService.SaveTokenAsync(tokenEntity);

            return Ok(new { Token = token });
        }

    }
}