using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

using MyNews.Api.Enums;
using MyNews.Api.Interfaces;

namespace MyNews.Api.Controllers
{
    /// <summary>
    /// Provides API endpoints for managing and retrieving user preferences
    /// related to sections and news sources.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserPreferencesController : ControllerBase
    {
        private readonly IUserPreferencesService _userPreferencesService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserPreferencesController"/> class.
        /// </summary>
        /// <param name="userPreferencesService">Service for managing user preferences.</param>
        public UserPreferencesController(IUserPreferencesService userPreferencesService)
        {
            _userPreferencesService = userPreferencesService;
        }

        /// <summary>
        /// Retrieves the currently selected sections for the authenticated user.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="SectionType"/> values representing the user’s selected sections.
        /// Returns <see cref="UnauthorizedResult"/> if the user ID cannot be determined.
        /// </returns>
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

        /// <summary>
        /// Updates the user's preferred sections.
        /// </summary>
        /// <param name="sectionIds">A list of section identifiers to set as selected.</param>
        /// <returns>
        /// A success message or <see cref="UnauthorizedResult"/> if the user is not authenticated.
        /// </returns>
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

        /// <summary>
        /// Updates the order of the user’s selected sections.
        /// </summary>
        /// <param name="sectionIds">
        /// A list of section identifiers representing the new display order.
        /// The order of the list corresponds to the desired order of sections on the frontend.
        /// </param>
        /// <returns>
        /// Returns <see cref="OkResult"/> on success or <see cref="UnauthorizedResult"/> 
        /// if the user’s identity cannot be determined.
        /// </returns>
        [HttpPost("update-sections-order")]
        public async Task<IActionResult> UpdateSectionsOrder([FromBody] List<int> sectionIds)
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized();

            await _userPreferencesService.UpdateSectionsOrderAsync(userId.Value, sectionIds);

            var updatedSections = await _userPreferencesService.GetSelectedSectionsAsync(userId.Value);

            return Ok(new { message = "Section order updated.", sections = updatedSections });
        }

        /// <summary>
        /// Retrieves the currently selected sources for the authenticated user.
        /// </summary>
        /// <returns>
        /// A collection of source IDs representing the user’s selected news sources.
        /// Returns <see cref="UnauthorizedResult"/> if the user ID cannot be determined.
        /// </returns>
        [HttpGet("sources")]
        public async Task<IActionResult> GetSelectedSources()
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized();

            var sources = await _userPreferencesService.GetSelectedSourcesAsync(userId.Value);
            return Ok(sources);
        }

        /// <summary>
        /// Updates the user's preferred news sources.
        /// </summary>
        /// <param name="sourceIds">A list of source IDs to set as selected.</param>
        /// <returns>
        /// A success message or <see cref="UnauthorizedResult"/> if the user is not authenticated.
        /// </returns>
        [HttpPost("sources")]
        public async Task<IActionResult> UpdateSources([FromBody] List<int> sourceIds)
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized();

            await _userPreferencesService.UpdateSourcesAsync(userId.Value, sourceIds);
            return Ok(new { message = "Sources updated." });
        }

        /// <summary>
        /// Extracts the authenticated user's unique identifier from the JWT claims.
        /// </summary>
        /// <returns>The user's ID as <see cref="Guid"/> or <c>null</c> if missing/invalid.</returns>
        private Guid? GetUserId()
        {
            var claim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(claim, out var userId))
            {
                return userId;
            }

            return null;
        }
    }
}
