using Microsoft.AspNetCore.Mvc;

using MyNews.Api.DTOs;
using MyNews.Api.Interfaces;

namespace MyNews.Api.Controllers
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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            var result = await _authService.LoginAsync(login);

            if (result == null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto register)
        {
            try
            {
                var result = await _authService.RegisterAsync(register);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPassword)
        {
            var result = await _authService.ForgotPasswordAsync(forgotPassword.Email);

            if (!result)
            {
                return BadRequest(new { message = "Email not found." });
            }

            return Ok(new { message = "Password reset link sent." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordDto resetPassword)
        {
            var result = await _authService.ResetPasswordAsync(resetPassword.Token, resetPassword.NewPassword);

            if (!result)
            {
                return BadRequest(new { message = "Invalid or expired token." });
            }

            return Ok(new { message = "Password reset successfully." });
        }
    }
}
