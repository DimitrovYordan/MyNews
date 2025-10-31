using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

using MyNews.Api.DTOs;
using MyNews.Api.Interfaces;

namespace MyNews.Api.Controllers
{
    /// <summary>
    /// Handles all user-related operations such as profile management,
    /// onboarding progress, and account deletion.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="userService">Service responsible for managing user operations.</param>
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Retrieves the currently authenticated user's profile.
        /// </summary>
        /// <returns>Returns <see cref="IActionResult"/> with user profile or error if not found.</returns>
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("Invalid user.");
            }

            var profile = await _userService.GetProfileAsync(userId);
            if (profile == null)
            {
                return NotFound("User not found.");
            }

            return Ok(profile);
        }

        /// <summary>
        /// Updates the current authenticated user's profile.
        /// </summary>
        /// <param name="dto">The data transfer object containing updated user information.</param>
        /// <returns>
        /// Returns success message if updated, 400 for invalid password match, 
        /// 404 if user not found, or 401 for unauthorized access.
        /// </returns>
        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateCurrentUser([FromBody] UpdateUserDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("Invalid user.");
            }

            var result = await _userService.UpdateProfileAsync(userId, dto);
            if (result == null)
            {
                return NotFound("User not found.");
            }

            if (result == "Passwords do not match.")
            {
                return BadRequest(result);
            }

            return Ok(new { message = result });
        }

        /// <summary>
        /// Marks the current authenticated user's account as deleted (soft delete).
        /// </summary>
        /// <returns>Returns success message if deleted, 404 or 401 otherwise.</returns>
        [HttpDelete("delete-profile")]
        public async Task<IActionResult> DeleteCurrentUser()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("Invalid user.");
            }

            var result = await _userService.DeleteUserAsync(userId);
            if (result == null)
            {
                return NotFound("User not found.");
            }

            return Ok(new { message = result });
        }

        /// <summary>
        /// Gets the onboarding completion status for the authenticated user.
        /// </summary>
        /// <returns>
        /// Returns a boolean value indicating if onboarding is completed, 
        /// or 404/401 for errors.
        /// </returns>
        [HttpGet("onboarding-status")]
        public async Task<IActionResult> GetOnboardingStatus()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("Invalid user.");
            }

            var status = await _userService.GetOnboardingStatusAsync(userId);
            if (status == null)
            {
                return NotFound("User not found.");
            }

            return Ok(new { isOnboardingComplete = status });
        }

        /// <summary>
        /// Marks the onboarding process as completed for the authenticated user.
        /// </summary>
        /// <returns>
        /// Returns success message if completed, or 404/401 if user is invalid.
        /// </returns>
        [HttpPost("complete-onboarding")]
        public async Task<IActionResult> CompleteOnboarding()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("Invalid user.");
            }

            var success = await _userService.MarkOnboardingCompletedAsync(userId);
            if (!success)
            {
                return NotFound("User not found.");
            }

            return Ok(new { message = "Onboarding marked as completed." });
        }
    }
}
