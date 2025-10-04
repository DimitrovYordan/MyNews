using Microsoft.AspNetCore.Mvc;

using MyNews.Api.DTOs;
using MyNews.Api.Interfaces;

namespace MyNews.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public ContactController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> Send([FromBody] ContactMessageDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _emailService.SendContactMessageAsync(dto.Title, dto.Message, dto.FromEmail);
            return Ok(new { success = true });
        }
    }
}
