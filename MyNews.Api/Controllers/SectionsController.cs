using Microsoft.AspNetCore.Mvc;

using MyNews.Api.Enums;

namespace MyNews.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<object>> GetSections()
        {
            var sections = Enum.GetValues(typeof(SectionType))
                .Cast<SectionType>()
                .Select(s => new { Id = (int)s, Name = s.ToString() })
                .ToList();

            return Ok(sections);
        }
    }
}
