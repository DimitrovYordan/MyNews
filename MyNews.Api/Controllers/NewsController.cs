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

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
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
    }
}
