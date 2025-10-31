using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Text.RegularExpressions;

using MyNews.Api.DTOs;
using MyNews.Api.Interfaces;

namespace MyNews.Api.Controllers
{
    /// <summary>
    /// Controller responsible for handling contact form submissions.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ContactController : ControllerBase
    {
        private readonly IEmailService _emailService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactController"/> class.
        /// </summary>
        /// <param name="emailService">Service used for sending emails.</param>
        public ContactController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        /// <summary>
        /// Receives a contact message from a client and sends it via email.
        /// </summary>
        /// <param name="dto">Contact message data transfer object.</param>
        /// <returns>Returns <see cref="IActionResult"/> with success or validation error.</returns>
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

        /// <summary>
        /// Removes potentially unsafe HTML or special characters from a plain text string.
        /// </summary>
        /// <param name="input">The raw text input.</param>
        /// <returns>Sanitized plain text string.</returns>
        private static string SanitizePlainText(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            string noHtml = Regex.Replace(input, "<.*?>", string.Empty);
            
            return noHtml.Replace("\r", "").Replace("\0", "");
        }
    }
}
