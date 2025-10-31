using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

using MyNews.Api.Interfaces;

namespace MyNews.Api.Controllers
{
    /// <summary>
    /// Provides endpoints for managing and retrieving available news sources.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SourcesController : ControllerBase
    {
        private readonly ISourceService _sourceService;
        private readonly IUserPreferencesService _userPreferencesService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SourcesController"/> class.
        /// </summary>
        /// <param name="sourceService">Service for accessing sources.</param>
        /// <param name="userPreferencesService">Service for managing user preferences.</param>
        public SourcesController(ISourceService sourceService, IUserPreferencesService userPreferencesService)
        {
            _sourceService = sourceService;
            _userPreferencesService = userPreferencesService;
        }

        /// <summary>
        /// Retrieves all available news sources along with user's selected preferences.
        /// </summary>
        /// <returns>
        /// A list of sources containing:
        /// <list type="bullet">
        /// <item><description><c>Id</c> – Source identifier.</description></item>
        /// <item><description><c>Name</c> – Source display name.</description></item>
        /// <item><description><c>IsSelected</c> – Indicates whether the user has selected this source.</description></item>
        /// </list>
        /// Returns <see cref="UnauthorizedResult"/> if the user ID cannot be resolved.
        /// </returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAllSources()
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized();

            var sources = await _sourceService.GetAllSources();
            var selected = await _userPreferencesService.GetSelectedSourcesAsync(userId.Value);

            var result = sources.Select(s => new
            {
                s.Id,
                s.Name,
                IsSelected = selected.Contains(s.Id),
            });

            return Ok(result);
        }

        /// <summary>
        /// Extracts the current user's unique identifier from the authentication claim.
        /// </summary>
        /// <returns>
        /// The user's <see cref="Guid"/> if available; otherwise, <c>null</c>.
        /// </returns>
        private Guid? GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(claim, out var userId))
                return userId;

            return null;
        }
    }
}
