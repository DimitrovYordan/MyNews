using Microsoft.AspNetCore.Mvc;

using MyNews.Api.Enums;
using MyNews.Api.Interfaces;

namespace MyNews.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            var result = await _newsService.GetNewsBySectionsAsync(sectionIds);

            return Ok(result);
        }

        [HttpGet("rss")]
        public async Task<IActionResult> GetRssNews()
        {
            var sources = await _sourceService.GetAllAsync();

            var news = await _rssService.FetchAndProcessRssFeedAsync(sources);

            return Ok(news);
        }
    }
}
