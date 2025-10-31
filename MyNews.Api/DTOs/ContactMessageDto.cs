using System.ComponentModel.DataAnnotations;

namespace MyNews.Api.DTOs
{
    /// <summary>
    /// Data transfer object representing a contact message sent from the client.
    /// </summary>
    public class ContactMessageDto
    {
        [Required, MaxLength(50)]
        public string Title { get; set; }

        [Required, EmailAddress, MaxLength(70)]
        public string? FromEmail { get; set; }

        [Required, MaxLength(1000)]
        public string Message { get; set; }
    }
}
