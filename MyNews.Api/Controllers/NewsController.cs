using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

using MyNews.Api.Enums;
using MyNews.Api.Interfaces;

namespace MyNews.Api.Controllers
{
    /// <summary>
    /// Handles all news-related API operations such as fetching news,
    /// marking articles as read, and retrieving RSS feeds.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;
        private readonly IRssService _rssService;
        private readonly ISourceService _sourceService;
        private readonly IUserPreferencesService _userPreferencesService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsController"/> class.
        /// </summary>
        public NewsController(INewsService newsService, IRssService rssService, ISourceService sourceService, IUserPreferencesService userPreferencesService)
        {
            _newsService = newsService;
            _rssService = rssService;
            _sourceService = sourceService;
            _userPreferencesService = userPreferencesService;
        }

        /// <summary>
        /// Retrieves a list of news items filtered by sections and sources.
        /// If no filters are provided, the user's preferences are used.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetNews([FromQuery] List<int> sectionIds, [FromQuery] List<int> sourceIds)
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized();

            var selectedSourceIds = (sourceIds != null && sourceIds.Any())
                ? sourceIds
                : (await _userPreferencesService.GetSelectedSourcesAsync(userId.Value))
                    .Select(s => (int)s)
                    .ToList();

            var selectedSectionIds = (sectionIds != null && sectionIds.Any())
                ? sectionIds
                : (await _userPreferencesService.GetSelectedSectionsAsync(userId.Value))
                    .Select(s => (int)s)
                    .ToList();

            var news = await _newsService.GetNewsBySectionsAndSourcesAsync(selectedSectionIds, selectedSourceIds);

            var result = news.Select(n => new 
            {
                n.Id,
                SectionId = (int)n.Section,
                SectionName = n.Section.ToString(),
                n.Title,
                n.Description,
                n.Summary,
                n.PublishedAt,
                n.Link,
                n.SourceName,
                n.SourceUrl,
                n.IsNew,
                n.IsRead,
                n.Translations
            });

            return Ok(result);
        }

        /// <summary>
        /// Retrieves news grouped by sections for a given user.
        /// </summary>
        [HttpPost("by-sections")]
        public async Task<IActionResult> GetNewsBySections([FromBody] List<SectionType> sectionIds)
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(claim, out var userId)) {
                return Unauthorized();
            }

            var result = await _newsService.GetNewsBySectionsAsync(sectionIds, userId);

            return Ok(result);
        }

        /// <summary>
        /// Fetches and processes RSS feeds from all configured sources.
        /// </summary>
        [HttpGet("rss")]
        public async Task<IActionResult> GetRssNews()
        {
            var sources = await _sourceService.GetAllSources();

            var news = await _rssService.FetchAndProcessRssFeedAsync(sources);

            return Ok(news);
        }

        /// <summary>
        /// Marks a news item as read for the current user.
        /// </summary>
        [HttpPost("mark-as-read/{newsItemId}")]
        public async Task<IActionResult> MarkAsRead(Guid newsItemId)
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(claim, out var userId))
                return Unauthorized();

            await _newsService.MarkAsReadAsync(userId, newsItemId);
            return Ok();
        }

        /// <summary>
        /// Marks that the current user has clicked a link from a news item.
        /// </summary>
        [HttpPost("mark-link-clicked/{newsItemId}")]
        public async Task<IActionResult> MarkLinkClicked(Guid newsItemId)
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(claim, out var userId))
                return Unauthorized();

            await _newsService.MarkLinkClickedAsync(userId, newsItemId);
            return Ok();
        }

        /// <summary>
        /// Extracts the user's ID from JWT claims.
        /// </summary>
        private Guid? GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(claim, out var userId))
                return userId;
            return null;
        }
    }
}
