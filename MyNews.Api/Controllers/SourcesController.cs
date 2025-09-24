using Microsoft.AspNetCore.Mvc;

using MyNews.Api.Enums;
using MyNews.Api.Interfaces;
using MyNews.Api.Models;

namespace MyNews.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SourcesController : ControllerBase
    {
        private readonly ISourceService _sourceService;

        public SourcesController(ISourceService sourceService)
        {
            _sourceService = sourceService;
        }

        // GET: api/sources/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Source>> GetSource(int id)
        {
            var source = await _sourceService.GetByIdAsync(id);

            if (source == null)
            {
                return NotFound();
            }

            return Ok(source);
        }

        [HttpGet("by-section/{section}")]
        public async Task<ActionResult<IEnumerable<object>>> GetSourcesBySection(SectionType section)
        {
            var sources = await _sourceService.GetBySectionAsync(section);

            var result = sources.Select(s => new
            {
                s.Id,
                s.Name,
                s.Url,
                s.Section,
                NewsCount = s.NewsItems.Count,
            });

            return Ok(result);
        }

        // POST: api/sources
        [HttpPost]
        public async Task<ActionResult<Source>> PostSource(Source source)
        {
            var createdSource = await _sourceService.CreateAsync(source);

            return CreatedAtAction(nameof(GetSource), new { id = createdSource.Id }, createdSource);
        }

        // PUT: api/sources/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSource(int id, Source source)
        {
            var updated = await _sourceService.UpdateAsync(id, source);
            if (!updated)
            {
                return BadRequest();
            }

            return NoContent();
        }

        // DELETE: api/sources/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSource(int id)
        {
            var deleted = await _sourceService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
