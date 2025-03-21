using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WordleBackend.Services;
using WordleBackend.Models;

namespace WordleBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var user = await _authService.RegisterAsync(request.Email, request.Password, request.Username);
                var token = await _authService.LoginAsync(request.Username, request.Password);
                return Ok(new { message = "ثبت‌نام با موفقیت انجام شد", token, username = user.Username });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var token = await _authService.LoginAsync(request.Username, request.Password);
                return Ok(new { token, username = request.Username });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("verify")]
        [Authorize]
        public IActionResult Verify()
        {
            try
            {
                var username = User.Identity?.Name;
                return Ok(new { username, isValid = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    public class RegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
} 