using Microsoft.AspNetCore.Mvc;

using MyNews.Api.DTOs;
using MyNews.Api.Interfaces;

namespace MyNews.Api.Controllers
{
    /// <summary>
    /// Controller responsible for handling user authentication and account management operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="authService">The authentication service used for login, registration, and password management.</param>
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token if the credentials are valid.
        /// </summary>
        /// <param name="login">The login data containing email and password.</param>
        /// <returns>A response with JWT token or Unauthorized result.</returns>
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

        /// <summary>
        /// Registers a new user account.
        /// </summary>
        /// <param name="register">The registration data including name, email, and password.</param>
        /// <returns>JWT token if registration succeeds or an error message if it fails.</returns>
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
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Initiates a password reset by sending an email with a reset link.
        /// </summary>
        /// <param name="forgotPassword">The object containing the user's email.</param>
        /// <returns>Ok if email sent, otherwise BadRequest.</returns>
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

        /// <summary>
        /// Resets a user’s password using a provided token.
        /// </summary>
        /// <param name="resetPassword">The data including reset token and new password.</param>
        /// <returns>Ok if successful, otherwise BadRequest.</returns>
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
