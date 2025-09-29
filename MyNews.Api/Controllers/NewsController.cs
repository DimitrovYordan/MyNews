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
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;
        private readonly IRssService _rssService;
        private readonly ISourceService _sourceService;

        public NewsController(INewsService newsService, IRssService rssService, ISourceService sourceService)
        {
            _newsService = newsService;
            _rssService = rssService;
            _sourceService = sourceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetNews([FromQuery] List<int> sectionsIds)
        {
            // Fetch all news items ordered by date descending
            var news = await _newsService.GetNewsAsync(sectionsIds);

            return Ok(news);
        }

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

        [HttpGet("rss")]
        public async Task<IActionResult> GetRssNews()
        {
            var sources = await _sourceService.GetAllAsync();

            var news = await _rssService.FetchAndProcessRssFeedAsync(sources);

            return Ok(news);
        }

        [HttpPost("mark-as-read/{newsItemId}")]
        public async Task<IActionResult> MarkAsRead(Guid newsItemId)
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(claim, out var userId))
                return Unauthorized();

            await _newsService.MarkAsReadAsync(userId, newsItemId);
            return Ok();
        }

        [HttpPost("mark-link-clicked/{newsItemId}")]
        public async Task<IActionResult> MarkLinkClicked(Guid newsItemId)
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(claim, out var userId))
                return Unauthorized();

            await _newsService.MarkLinkClickedAsync(userId, newsItemId);
            return Ok();
        }
    }
}
