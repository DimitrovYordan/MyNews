using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using MyNews.Api.Data;
using MyNews.Api.DTOs;
using MyNews.Api.Enums;

namespace MyNews.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NewsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetNews([FromQuery] List<int> sectionsIds)
        {
            // Fetch all news items ordered by date descending
            var news = _context.NewsItems
                .Where(n => sectionsIds.Count == 0 || sectionsIds.Contains((int)n.Section))
                .ToList();

            return Ok(news);
        }

        [HttpPost("by-sections")]
        public async Task<IActionResult> GetNewsBySections([FromBody] List<SectionType> sectionIds)
        {
            var selectedSections = sectionIds.Any()
                ? sectionIds
                : Enum.GetValues(typeof(SectionType)).Cast<SectionType>().ToList();

            var result = new List<SectionWithNewsDto>();

            foreach (var section in selectedSections)
            {
                var news = await _context.NewsItems
                    .Where(n => n.Section == section)
                    .Select(n => new NewsItemDto
                    {
                        Id = n.Id,
                        Title = n.Title,
                        Content = n.Content,
                        PublishedAt = n.PublishedAt
                    })
                    .ToListAsync();

                result.Add(new SectionWithNewsDto
                {
                    SectionId = (int)section,
                    SectionName = section.ToString(),
                    News = news
                });
            }

            return Ok(result);
        }
    }
}
