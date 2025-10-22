using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

using MyNews.Api.Interfaces;

namespace MyNews.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SourcesController : ControllerBase
    {
        private readonly ISourceService _sourceService;
        private readonly IUserPreferencesService _userPreferencesService;

        public SourcesController(ISourceService sourceService, IUserPreferencesService userPreferencesService)
        {
            _sourceService = sourceService;
            _userPreferencesService = userPreferencesService;
        }

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

        private Guid? GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(claim, out var userId))
                return userId;

            return null;
        }
    }
}
