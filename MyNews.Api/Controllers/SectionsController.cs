using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

using MyNews.Api.Enums;
using MyNews.Api.Interfaces;

namespace MyNews.Api.Controllers
{
    /// <summary>
    /// API controller responsible for returning available sections and marking which ones
    /// are selected by the currently authenticated user.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SectionsController : ControllerBase
    {
        private readonly ISectionsService _sectionsService;
        private readonly IUserPreferencesService _userPreferencesService;

        public SectionsController(ISectionsService sectionsService, IUserPreferencesService userPreferencesService)
        {
            _sectionsService = sectionsService;
            _userPreferencesService = userPreferencesService;
        }

        /// <summary>
        /// Returns all available news sections and indicates whether each section
        /// is selected by the current user.
        /// </summary>
        /// <returns>A list of sections with selection flags.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAllSections()
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized();

            var sections = await _sectionsService.GetAllSections();
            var selected = await _userPreferencesService.GetSelectedSectionsAsync(userId.Value);

            var result = sections.Select(s => new
            {
                s.Id,
                s.Name,
                IsSelected = selected.Contains((SectionType)s.Id)
            });

            return Ok(result);
        }

        private Guid? GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(claim, out var userId))
                return userId;

            return null;
        }
    }
}
