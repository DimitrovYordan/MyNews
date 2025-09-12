using Microsoft.AspNetCore.Mvc;

using MyNews.Api.Interfaces;

namespace MyNews.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionsController : ControllerBase
    {
        private readonly ISectionsService _sectionsService;

        public SectionsController(ISectionsService sectionsService)
        {
            _sectionsService = sectionsService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<object>> GetSections()
        {
            var sections = _sectionsService.GetSections();

            return Ok(sections);
        }
    }
}
