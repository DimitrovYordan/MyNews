using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using MyNews.Api.Data;
using MyNews.Api.Models;

namespace MyNews.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SourcesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SourcesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/sources
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Source>>> GetSources()
        {
            return await _context.Sources.ToListAsync();
        }

        // GET: api/sources/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Source>> GetSource(int id)
        {
            var source = await _context.Sources.FindAsync(id);

            if (source == null)
                return NotFound();

            return source;
        }

        // POST: api/sources
        [HttpPost]
        public async Task<ActionResult<Source>> PostSource(Source source)
        {
            _context.Sources.Add(source);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSource), new { id = source.Id }, source);
        }

        // PUT: api/sources/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSource(int id, Source source)
        {
            if (id != source.Id)
                return BadRequest();

            _context.Entry(source).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/sources/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSource(int id)
        {
            var source = await _context.Sources.FindAsync(id);
            if (source == null)
                return NotFound();

            _context.Sources.Remove(source);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
