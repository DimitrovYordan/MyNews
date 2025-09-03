using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using MyNews.Api.Data;
using MyNews.Api.Models;

namespace MyNews.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Constructor injects the database context
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

        [HttpPost]
        public async Task<IActionResult> AddNews([FromBody] NewsItem newsItem)
        {
            // Add new news item to the database
            _context.NewsItems.Add(newsItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNews), new { id = newsItem.Id }, newsItem);
        }

        [HttpGet("bySections")]
        public async Task<ActionResult<IEnumerable<NewsItem>>> GetNewsBySections([FromQuery] List<int> sectionIds)
        {
            var news = await _context.NewsItems
                .Where(n => sectionIds.Contains((int)n.Section))
                .ToListAsync();

            return Ok(news);
        }
    }
}
