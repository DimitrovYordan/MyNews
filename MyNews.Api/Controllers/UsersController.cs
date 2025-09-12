using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using MyNews.Api.DTOs;
using MyNews.Api.Interfaces;

using System.Security.Claims;

namespace MyNews.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

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
    }
}
