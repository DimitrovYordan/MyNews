using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MyNews.Api.DTOs;
using MyNews.Api.Interfaces;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;

namespace MyNews.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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

            var safeTitle = SanitizePlainText(dto.Title);
            var safeMessage = SanitizePlainText(dto.Message);
            var safeEmail = SanitizePlainText(dto.FromEmail ?? string.Empty);

            await _emailService.SendContactMessageAsync(safeTitle, safeMessage, safeEmail);

            return Ok(new { success = true });
        }

        private static string SanitizePlainText(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            string noHtml = Regex.Replace(input, "<.*?>", string.Empty);
            
            return noHtml.Replace("\r", "").Replace("\0", "");
        }
    }
}
