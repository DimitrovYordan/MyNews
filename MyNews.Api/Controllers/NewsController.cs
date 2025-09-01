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
        public async Task<IActionResult> GetNews()
        {
            // Fetch all news items ordered by date descending
            var news = await _context.NewsItems
                                     .OrderByDescending(n => n.PublishedAt)
                                     .ToListAsync();

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
    }
}
