using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

using MyNews.Api.Enums;
using MyNews.Api.Interfaces;

namespace MyNews.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserPreferencesController : ControllerBase
    {
        private readonly IUserPreferencesService _userPreferencesService;

        public UserPreferencesController(IUserPreferencesService userPreferencesService)
        {
            _userPreferencesService = userPreferencesService;
        }

        [HttpGet("sections")]
        public async Task<IActionResult> GetSelectedSections()
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var sections = await _userPreferencesService.GetSelectedSectionsAsync(userId.Value);

            return Ok(sections);
        }

        [HttpPost("sections")]
        public async Task<IActionResult> UpdateSections([FromBody] List<SectionType> sectionIds)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            await _userPreferencesService.UpdateSectionsAsync(userId.Value, sectionIds);

            return Ok(new { message = "Sections updated." });
        }

        private Guid? GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(claim, out var userId))
            {
                return userId;
            }

            return null;
        }
    }
}
